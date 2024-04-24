using Azure;
using GoogleMaps.LocationServices;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Common.AssetManagement.Common;
//using TaxiBookingService.Common.Enums;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Dal.Repositories;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;
using AutoMapper;
using TaxiBookingService.API.Ride;
using TaxiBookingService.Client.Geocoding.Interfaces;
using Azure.Core;

namespace TaxiBookingService.Logic.User
{
    public class DriverLogic : IDriverLogic<Driver>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IExternalHttpClient _externalApiClient;

        public DriverLogic(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper, IExternalHttpClient externalApiClient
)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _externalApiClient = externalApiClient;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private bool VerifyPasswordHash(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512(passwordSalt))
            {
                var computedHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return computedHash.SequenceEqual(passwordHash);
            }
        }

        private string CreateToken(Driver driver)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, driver.User.Email),
                new Claim(ClaimTypes.Role, "Driver")
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration.GetSection("AppSettings:Token").Value));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);

            var token = new JwtSecurityToken(
                claims: claims,
                expires: DateTime.Now.AddMinutes(15),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        private RefreshToken GenerateRefreshToken()
        {
            return new RefreshToken
            {
                Token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64)),
                Expires = DateTime.UtcNow.AddDays(1)
            };
        }

        private async Task SetRefreshToken(Driver driver, RefreshToken newRefreshToken)
        {
            await _unitOfWork.DriverRepository.UpdateRefreshToken(driver, newRefreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }

        public async Task<int> Register(DriverRegisterDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var result = await _unitOfWork.DriverRepository.Register(request, passwordHash, passwordSalt);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<string> Login(DriverLoginDto request)
        {
            var driver = await _unitOfWork.DriverRepository.GetByEmail(request.Email);
            if (driver == null)
            {
                throw new Exception(AppConstant.UserNotFound);
            }



            if (!VerifyPasswordHash(request.Password, driver.User.PasswordHash, driver.User.PasswordSalt))
            {
                throw new AuthenticationException(AppConstant.PasswordNotFound);
            }

            await _unitOfWork.DriverRepository.Login(driver);
            await _unitOfWork.SaveChangesAsync();

            string token = CreateToken(driver);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);

            var refreshToken = GenerateRefreshToken();
            await SetRefreshToken(driver, refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return token;
        }

        public async Task<string> RefreshToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var driver = await _unitOfWork.DriverRepository.GetByToken(refreshToken);

            if (driver == null)
            {
                throw new InvalidTokenException(AppConstant.InvalidToken);
            }
            else if (driver.User.TokenExpires <= DateTime.Now)
            {
                throw new TokenExpiredException(AppConstant.TokenExpired);
            }

            string token = CreateToken(driver);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);

            var newRefreshToken = GenerateRefreshToken();
            await SetRefreshToken(driver, newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return token;
        }

        public async Task Logout()
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var driver = await _unitOfWork.DriverRepository.GetByToken(loggedInUser);
            if (driver == null)
            {
                throw new Exception(AppConstant.UserNotFound);
            }

            await _unitOfWork.DriverRepository.Logout(driver);
            await _unitOfWork.SaveChangesAsync();

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");
        }

        public async Task<int> AddTaxi(DriverTaxiDto request)
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var driver = await _unitOfWork.DriverRepository.GetByToken(loggedInUser);
            var taxiId = await _unitOfWork.TaxiRepository.AddTaxi(request, driver.Id);
            await _unitOfWork.SaveChangesAsync();
            return taxiId;
        }

        public async Task<decimal> CalculateFare(int rideId)
        {
            var ride = await _unitOfWork.RideRepository.GetById(rideId);
            var pickUpLocation = await _unitOfWork.LocationRepository.GetById(ride.PickupLocationId);
            double distance = CalculateDistance(ride.PickupLocation.Latitude, ride.PickupLocation.Longitude, ride.DropoffLocation.Latitude, ride.DropoffLocation.Longitude);
            var tariff = await _unitOfWork.TariffChargeRepository.GetAll();
            decimal baseFare = tariff.FirstOrDefault(t => t.Name == "Basefare")?.Value ?? 0m;
            decimal perKmCharge = tariff.FirstOrDefault(t => t.Name == "PerKm")?.Value ?? 0m;
            decimal fare = baseFare + (decimal)distance * perKmCharge;
            return fare;
        }


        private double CalculateDistance(decimal customerLat, decimal customerLong, decimal driverLat, decimal driverLong)
        {
            double customerLatRad = ToRadians(Convert.ToDouble(customerLat));
            double customerLongRad = ToRadians(Convert.ToDouble(customerLong));
            double driverLatRad = ToRadians(Convert.ToDouble(driverLat));
            double driverLongRad = ToRadians(Convert.ToDouble(driverLong));
            double dlong = driverLongRad - customerLongRad;
            double dlat = driverLatRad - customerLatRad;
            double ans = Math.Pow(Math.Sin(dlat / 2), 2) +
                             Math.Cos(customerLatRad) * Math.Cos(driverLatRad) *
                             Math.Pow(Math.Sin(dlong / 2), 2);
            ans = 2 * Math.Asin(Math.Sqrt(ans));
            double R = 6371;
            ans = ans * R;
            return ans;
        }

        private double ToRadians(double angle)
        {
            return Math.PI * angle / 180.0;
        }



        public async Task<string> Accept(int driverId, int rideId)
        {

            await _unitOfWork.RideRepository.UpdateRideStatus(rideId,3,2);
            var driver = await _unitOfWork.DriverRepository.GetById(driverId);
            var fare = await CalculateFare(rideId);
           
            var taxi = await _unitOfWork.TaxiRepository.GetTaxiByDriverId(driverId);
            await _unitOfWork.SaveChangesAsync();
            return $"Driver Accepted:\n" +
                      $"- Name: {driver.User.Name}\n" +
                      $"- Contact Number: {driver.User.PhoneNumber}\n" +
                      $"- Taxi Type: {taxi.Name}\n" +
                      $"- Registration Number: {taxi.RegistrationNumber}\n" +
                      $"- Fare: {fare:F2}";
           

        }

        public async Task StartRide(int rideId)
        {
            await _unitOfWork.RideRepository.UpdateRideStatus(rideId, 3, 2);
        }

        public async Task<decimal> EndRide(int rideId)
        {
            await _unitOfWork.RideRepository.UpdateRideStatus(rideId, 4, 3);
            var ride = await _unitOfWork.RideRepository.GetById(rideId);
            decimal fare = await CalculateFare(rideId);
            await _unitOfWork.DriverRepository.UpdateStatus(ride.DriverId.Value, 1);

            //await _unitOfWork.CustomerRepository.UpdateWallet(fare,ride.CustomerId);
            //var tariff = await _unitOfWork.TariffChargeRepository.GetTariffCharges();
            //decimal commissionPercentage = tariff.FirstOrDefault(t => t.Name == "driverCommissionRate")?.Value ?? 0m;
            //decimal commissionAmount = (fare * commissionPercentage) / 100;
            return fare;

        }

        public async Task Decline(DriverDeclineDto request)
        {
            await _unitOfWork.DriverRepository.UpdateStatus(request.DriverId, 1);
            var declineentity = _mapper.Map<RejectedRide>(request);
            await _unitOfWork.RejectedRideRepository.Add(declineentity);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task CancelRide(int rideId, string reason)
        {
            var status = await _unitOfWork.RideRepository.GetStatus(rideId);
            var ride = await _unitOfWork.RideRepository.GetById(rideId);

            if (status == 2)
            {
                bool isValidReason = await IsValidCancellationReason(reason);

                if (!isValidReason)
                {

                    var tariff = await _unitOfWork.TariffChargeRepository.GetAll();

                    decimal CancellationFee = tariff.FirstOrDefault(t => t.Name == "CancellationFee")?.Value ?? 0m;
                    decimal rideEarnings = await CalculateFare(rideId);
                    decimal cancellationFee = rideEarnings * (CancellationFee / 100);

                    var driver = await _unitOfWork.DriverRepository.GetById(ride.DriverId.Value);
                    driver.Driverearnings -= cancellationFee;
                    await _unitOfWork.DriverRepository.Update(driver);
                }
                await _unitOfWork.RideRepository.UpdateRideStatus(rideId, 5, 2);
                await _unitOfWork.DriverRepository.UpdateStatus(ride.DriverId.Value, 1);
            }
            else
            {
                throw new CannotCancel(AppConstant.CannotCancel);
            }
        }

        public async Task UpdateCustomerRating(int customerId)
        {
            var ratings = await _unitOfWork.DriverRatingRepository.GetRatingsByCustomerId(customerId);

            if (ratings.Any())
            {
                float averageRating = ratings.Average(r => r.Rating).Value;
                var customer = await _unitOfWork.CustomerRepository.GetById(customerId);
                if (customer != null)
                {
                    customer.CustomerRating = averageRating;
                    await _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }

        private async Task<bool> IsValidCancellationReason(string reason)
        {
            var validReasons = await _unitOfWork.RideCancellationReasonRepository.GetAllValidReasons();
            return validReasons.Any(r => string.Equals(r.Name, reason, StringComparison.OrdinalIgnoreCase));
        }

        public async Task FeedBack(DriverRatingDto Feedback)
        {
            var ride = await _unitOfWork.RideRepository.Exists(Feedback.RideId);
            if (!ride)
            {
                throw new NotFoundException(AppConstant.RideNotFound);
            }
            var feedback = _mapper.Map<DriverRating>(Feedback);
            await _unitOfWork.DriverRatingRepository.Add(feedback);
            await _unitOfWork.SaveChangesAsync();
            var customerId = await _unitOfWork.RideRepository.GetCustomerByRideId(Feedback.RideId);
            await UpdateCustomerRating(customerId);
        }

        public async Task<List<DriverRideDisplayDto>> RideHistory()
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var driver = await _unitOfWork.DriverRepository.GetByToken(loggedInUser);
            var rides = await _unitOfWork.RideRepository.GetAllDriverRides(driver.Id);
            if (rides.Count == 0)
            {
                throw new NotFoundException(AppConstant.Notrides);
            }
            var rideHistoryDtoList = new List<DriverRideDisplayDto>();
            foreach (var ride in rides)
            {
                var pickUpAddress = await _externalApiClient.GetReverseGeocodingAsync("4a87e7d383bb4ca7a8c484db00f43434", ride.PickupLocation.Latitude, ride.PickupLocation.Longitude);
                var dropoffAddress = await _externalApiClient.GetReverseGeocodingAsync("4a87e7d383bb4ca7a8c484db00f43434", ride.DropoffLocation.Latitude, ride.DropoffLocation.Longitude);
                var rideHistoryDto = new DriverRideDisplayDto
                {
                    Id = ride.Id,
                    Customer = ride.Customer.User.Name,
                    TaxiType = ride.TaxiType.Name,
                    PickupLocation = pickUpAddress,
                    DropoffLocation = dropoffAddress,
                    StartTime = ride.StartTime,
                    EndTime = ride.EndTime,
                };

                rideHistoryDtoList.Add(rideHistoryDto);
            }
            return rideHistoryDtoList;
        }
    }
}

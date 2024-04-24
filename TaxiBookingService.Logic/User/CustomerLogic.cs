using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
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
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Client.Geocoding.Interfaces;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Logic.User
{
    public class CustomerLogic : ICustomerLogic<Customer>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IExternalHttpClient _externalApiClient;
        
        private readonly IMapper _mapper;

        public CustomerLogic(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IExternalHttpClient externalApiClient, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _externalApiClient = externalApiClient;
            _mapper = mapper;

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

        private string CreateToken(Customer Customer)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Email, Customer.User.Email),
                new Claim(ClaimTypes.Role, "Customer"), //need to modify

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

        private async Task SetRefreshToken(Customer Customer, RefreshToken newRefreshToken)
        {
            await _unitOfWork.CustomerRepository.UpdateRefreshToken(Customer, newRefreshToken);

            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Expires = newRefreshToken.Expires,
            };

            _httpContextAccessor.HttpContext.Response.Cookies.Append("refreshToken", newRefreshToken.Token, cookieOptions);
        }

        public async Task<int> Register(CustomerRegisterDto request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var userEntity = _mapper.Map<Dal.Entities.User>(request);
            var result = await _unitOfWork.CustomerRepository.Register(userEntity, passwordHash, passwordSalt);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<(string, string)> Login(CustomerLoginDto request)
        {
            var Customer = await _unitOfWork.CustomerRepository.GetByEmail(request.Email);
            if (Customer == null)
            {
                throw new Exception(AppConstant.UserNotFound);
            }
            if (!VerifyPasswordHash(request.Password, Customer.User.PasswordHash, Customer.User.PasswordSalt))
            {
                throw new AuthenticationException(AppConstant.PasswordNotFound);
            }

            await _unitOfWork.CustomerRepository.Login(Customer);
            await _unitOfWork.SaveChangesAsync();

            string token = CreateToken(Customer);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);

            var refreshToken = GenerateRefreshToken();
            await SetRefreshToken(Customer, refreshToken);
            await _unitOfWork.SaveChangesAsync();

            return (token, refreshToken.Token);
        }

        public async Task<string> RefreshToken()
        {
            var refreshToken = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var Customer = await _unitOfWork.CustomerRepository.GetByToken(refreshToken);

            if (Customer == null)
            {
                throw new InvalidTokenException(AppConstant.InvalidToken);
            }
            else if (Customer.User.TokenExpires <= DateTime.Now)
            {
                throw new TokenExpiredException(AppConstant.TokenExpired);
            }

            string token = CreateToken(Customer);
            _httpContextAccessor.HttpContext.Response.Cookies.Append("accessToken", token);

            var newRefreshToken = GenerateRefreshToken();
            await SetRefreshToken(Customer, newRefreshToken);
            await _unitOfWork.SaveChangesAsync();

            return token;
        }

        public async Task Logout()
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var Customer = await _unitOfWork.CustomerRepository.GetByToken(loggedInUser);
            if (Customer == null)
            {
                throw new Exception(AppConstant.UserNotFound);
            }

            await _unitOfWork.CustomerRepository.Logout(Customer);
            await _unitOfWork.SaveChangesAsync();

            _httpContextAccessor.HttpContext.Response.Cookies.Delete("refreshToken");
            _httpContextAccessor.HttpContext.Response.Cookies.Delete("accessToken");
        }

        public async Task<int> BookRide(CustomerBookRideDto request)
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var Customer = await _unitOfWork.CustomerRepository.GetByToken(loggedInUser);
            var PickupLocation= await _externalApiClient.GetGeocodingAsync("4a87e7d383bb4ca7a8c484db00f43434", request.PickupLocation);
            var DropoffLocation = await _externalApiClient.GetGeocodingAsync("4a87e7d383bb4ca7a8c484db00f43434", request.DropoffLocation);
            var fare = await CalculateFare(PickupLocation.Latitude, PickupLocation.Longitude, DropoffLocation.Latitude, DropoffLocation.Longitude);
            var result = await _unitOfWork.RideRepository.BookRide(PickupLocation,DropoffLocation, request, Customer.Id);
            await _unitOfWork.PaymentRepository.CreatePayment(result,request.PaymentType,fare);
            await _unitOfWork.SaveChangesAsync();    
            return result;
        }

        public async Task CancelRide(int rideId, string reason)
        {
            var status = await _unitOfWork.RideRepository.GetStatus(rideId);
            var ride = await _unitOfWork.RideRepository.GetById(rideId);

            if (status == 2 || status==1)
            {
                bool isValidReason = await IsValidCancellationReason(reason);
                if (!isValidReason)
                {
                    var tariff = await _unitOfWork.TariffChargeRepository.GetAll();
                    decimal CancellationFee = tariff.FirstOrDefault(t => t.Name == "CancellationFee")?.Value ?? 0m;
                    decimal rideEarnings = await _unitOfWork.PaymentRepository.GetFareByRide(rideId);
                    decimal cancellationFee = rideEarnings * (CancellationFee / 100);
                    var customer = await _unitOfWork.CustomerRepository.GetById(ride.CustomerId);
                    customer.Customerwallet -= cancellationFee;
                    await _unitOfWork.CustomerRepository.Update(customer);
                }
                await _unitOfWork.RideRepository.UpdateRideStatus(rideId, 5, 2);
            }
            else
            {
                throw new CannotCancel(AppConstant.CannotCancel);
            }
        }

        private async Task<bool> IsValidCancellationReason(string reason)
        {
            var validReasons = await _unitOfWork.RideCancellationReasonRepository.GetAllValidReasons();
            return validReasons.Any(r => string.Equals(r.Name, reason, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<decimal> CalculateFare(decimal pickUpLat, decimal pickUpLong, decimal dropOffLat, decimal dropOffLong)
        {
        
            double distance = CalculateDistance(pickUpLat, pickUpLong, dropOffLat,dropOffLong);
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


        public async Task<Driver> FindNearbyDriverAsync(int rideId)
        {
            var rideStatus = await _unitOfWork.RideRepository.GetStatus(rideId);
            if (rideStatus != 1)
            {
                throw new Exception("Ride is not in the searching state.");
            }
            var pickUpLoc = await _unitOfWork.RideRepository.GetRideLongLat(rideId);
            var allDrivers = await _unitOfWork.DriverRepository.GetAllTaxiTypeDrivers(rideId);//unavaliable
            var allTaxiTypeDrivers = await _unitOfWork.DriverRepository.GetAllDrivers();
            var nearbyDrivers = new List<Driver>();
            int maxDistance = 5;
            foreach (var driver in allDrivers)
            {
                var driverLoc = await _unitOfWork.DriverRepository.GetLongLat(driver.UserId);
                double distance = CalculateDistance(pickUpLoc.latitude, pickUpLoc.longitude, driverLoc.latitude, driverLoc.longitude);
                if (distance < maxDistance)
                {
                    bool hasRejected = await _unitOfWork.RejectedRideRepository.HasDriverRejectedRide(driver.Id, rideId);
                    if (!hasRejected)
                    {
                        nearbyDrivers.Add(driver);
                    }
                }
            }
            nearbyDrivers = nearbyDrivers.OrderBy(x => x.DriverRating).ToList();

            return nearbyDrivers[0];
        }

        public async Task<Driver> GetDriverAsync(int id)
        {
            var driver = await FindNearbyDriverAsync(id);
            await _unitOfWork.RideRepository.GetById(id);
            await _unitOfWork.DriverRepository.UpdateStatus(driver.Id, 2);
            await _unitOfWork.RideRepository.UpdateRideStatus(id,2,1);
            await _unitOfWork.SaveChangesAsync();
            return driver;
        }

        public async Task UpdateDriverRating(int driverId)
        {
            var ratings = await _unitOfWork.CustomerRatingRepository.GetRatingsByDriverId(driverId);

            if (ratings.Any())
            {
                float averageRating = ratings.Average(r=>r.Rating).Value;

                var driver = await _unitOfWork.DriverRepository.GetById(driverId);
                if (driver != null)
                {
                    driver.DriverRating = averageRating;
                    await _unitOfWork.DriverRepository.Update(driver);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }


        public async Task FeedBack(CustomerRatingDto Feedback)
        {
            var ride = await _unitOfWork.RideRepository.Exists(Feedback.RideId);
            if (!ride)
            {
                throw new NotFoundException(AppConstant.RideNotFound);
            }
            var feedback = _mapper.Map<CustomerRating>(Feedback);

            await _unitOfWork.CustomerRatingRepository.Add(feedback);
            await _unitOfWork.SaveChangesAsync();
            var driverId=await _unitOfWork.RideRepository.GetDriverByRideId(Feedback.RideId);
            await UpdateDriverRating(driverId);
        }
        public async Task<List<CustomerRideDisplayDto>> RideHistory()
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var customer = await _unitOfWork.CustomerRepository.GetByToken(loggedInUser);
            var rides = await _unitOfWork.RideRepository.GetAllCustomerRides(customer.Id);
            if (rides.Count == 0)
            {
                throw new NotFoundException(AppConstant.Notrides);
            }
            var rideHistoryDtoList = new List<CustomerRideDisplayDto>();
            foreach (var ride in rides)
            {
                var pickUpAddress = await _externalApiClient.GetReverseGeocodingAsync("4a87e7d383bb4ca7a8c484db00f43434", ride.PickupLocation.Latitude, ride.PickupLocation.Longitude);
                var dropoffAddress = await _externalApiClient.GetReverseGeocodingAsync("4a87e7d383bb4ca7a8c484db00f43434", ride.DropoffLocation.Latitude, ride.DropoffLocation.Longitude);
                var rideHistoryDto = new CustomerRideDisplayDto
                {
                    Id = ride.Id,
                    Driver = ride.Driver.User.Name,
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

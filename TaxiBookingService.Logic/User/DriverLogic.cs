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
using GoogleMaps.LocationServices;

namespace TaxiBookingService.Logic.User
{
    public class DriverLogic : IDriverLogic<Driver>
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;

        public DriverLogic(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<int> Register(DriverRegisterServiceContracts request)
        {
            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var result = await _unitOfWork.DriverRepository.Register(request, passwordHash, passwordSalt);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<string> Login(DriverLoginServiceContracts request)
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

        public async Task<int> AddTaxi(DriverTaxiServiceContracts taxi)
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var driver = await _unitOfWork.DriverRepository.GetByToken(loggedInUser);
            var result = await _unitOfWork.TaxiRepository.AddTaxi(taxi, driver.Id);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task UpdateTaxi(int taxiId, DriverTaxiServiceContracts taxi)
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies["refreshToken"];
            var driver = await _unitOfWork.DriverRepository.GetByToken(loggedInUser);

            if (driver == null)
            {
                throw new Exception(AppConstant.UserNotFound);
            }
            var existingTaxi = await _unitOfWork.TaxiRepository.GetTaxiById(taxiId);
            if (existingTaxi == null || existingTaxi.DriverId != driver.Id)
            {
                throw new Exception("Taxi not found.");
            }
            await _unitOfWork.TaxiRepository.UpdateTaxi(taxiId, taxi);
        }

        public  async Task<decimal> CalculateFare(int rideId)
        {
            var ride = await _unitOfWork.RideRepository.GetById(rideId);
            var pickUpLocation = await _unitOfWork.LocationRepository.GetById(ride.PickupLocationId);

            double distance = CalculateDistance(ride.PickupLocation.Latitude, ride.PickupLocation.Longitude, ride.DropoffLocation.Latitude, ride.DropoffLocation.Longitude);
            
            var tariff = await _unitOfWork.TariffChargeRepository.GetTariffCharges();

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
            await _unitOfWork.DriverRepository.UpdateRideStatus(driverId,rideId);
            var driver=await _unitOfWork.DriverRepository.GetById(driverId);
            var fare=await CalculateFare(rideId);
            var taxi= await _unitOfWork.TaxiRepository.GetTaxiByDriverId(driverId);
            return $"Driver Accepted:\n" +
                      $"- Name: {driver.User.Name}\n" +
                      $"- Contact Number: {driver.User.PhoneNumber}\n" +
                      $"- Taxi Type: {taxi.Name}\n" +
                      $"- Registration Number: {taxi.RegistrationNumber}\n" +
                      $"- Fare: {fare:F2}";

        }

        public async Task StartRide(int rideId)
        {
            await  _unitOfWork.RideRepository.UpdateRideStatus(rideId,3,2);
        }

        public async Task<decimal> EndRide(int rideId)
        {
            await _unitOfWork.RideRepository.UpdateRideStatus(rideId,4,3);
            var ride = await _unitOfWork.RideRepository.GetById(rideId);
            decimal fare = await CalculateFare(rideId);
            await _unitOfWork.DriverRepository.UpdateStatus(ride.DriverId.Value, 1);

            //await _unitOfWork.CustomerRepository.UpdateWallet(fare,ride.CustomerId);
            //var tariff = await _unitOfWork.TariffChargeRepository.GetTariffCharges();
            //decimal commissionPercentage = tariff.FirstOrDefault(t => t.Name == "driverCommissionRate")?.Value ?? 0m;
            //decimal commissionAmount = (fare * commissionPercentage) / 100;
            return fare;

        }

        public async Task Decline(int DriverId, int rideId)
        {

            await _unitOfWork.DriverRepository.UpdateStatus(DriverId, 1);
            var result=await _unitOfWork.RejectedRideRepository.Add(DriverId, rideId);

            
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
                   
                    var tariff = await _unitOfWork.TariffChargeRepository.GetTariffCharges();

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
        }

        private async Task<bool> IsValidCancellationReason(string reason)
        {
            var validReasons = await _unitOfWork.RideCancellationReasonRepository.GetAllValidReasons();
            return validReasons.Any(r => string.Equals(r.Name, reason, StringComparison.OrdinalIgnoreCase));
        }

    }
}

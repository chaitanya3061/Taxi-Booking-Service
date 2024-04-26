using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Client.Geocoding.Interfaces;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Logic.User
{
    public class RideLogic :IRideLogic
    {
        private readonly IUnitOfWork _unitOfWork;
        public RideLogic(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<Driver> FindNearbyDriverAsync(int rideId)
        {
            var rideStatus = await _unitOfWork.RideRepository.GetStatus(rideId);
            if (rideStatus != 1)
            {
                throw new Exception("Ride is not in the searching state.");
            }
            var pickUpLoc = await _unitOfWork.RideRepository.GetRideLongLat(rideId);
            var allDrivers = await _unitOfWork.DriverRepository.GetAllTaxiTypeDrivers(rideId);
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
            nearbyDrivers = nearbyDrivers.OrderByDescending(x => x.DriverRating).ToList();
            return nearbyDrivers[0];
        }

        public async Task GetDriverAsync(int rideid)
        {
            var driver = await FindNearbyDriverAsync(rideid);
            var ride = await _unitOfWork.RideRepository.GetById(rideid);//handle 0
            ride.DriverId = driver.Id;
            driver.DriverStatusId = 2;
            await _unitOfWork.DriverRepository.Update(driver);
            await _unitOfWork.RideRepository.Update(ride);
            await _unitOfWork.SaveChangesAsync();
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


        public async Task<decimal> CalculateFare(decimal pickUpLat, decimal pickUpLong, decimal dropOffLat, decimal dropOffLong)
        {

            double distance = CalculateDistance(pickUpLat, pickUpLong, dropOffLat, dropOffLong);
            var tariff = await _unitOfWork.TariffChargeRepository.GetAll();
            decimal baseFare = tariff.FirstOrDefault(t => t.Name == "Basefare")?.Value ?? 0m;
            decimal perKmCharge = tariff.FirstOrDefault(t => t.Name == "PerKm")?.Value ?? 0m;
            decimal fare = baseFare + (decimal)distance * perKmCharge;
            return fare;
        }

    }
}

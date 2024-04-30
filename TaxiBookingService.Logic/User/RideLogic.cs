using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Logic.User
{
    public class RideLogic : IRideLogic
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILoggerAdapter _loggerAdapter;

        public RideLogic(IUnitOfWork unitOfWork, ILoggerAdapter loggerAdapter)
        {
            _unitOfWork = unitOfWork;
            _loggerAdapter = loggerAdapter;
        }

        private double CalculateDistance(Location pickUpLocation, Location dropOffLocation)
        {
            double pickUpLatRad = ToRadians(Convert.ToDouble(pickUpLocation.Latitude));
            double pickUpLongRad = ToRadians(Convert.ToDouble(pickUpLocation.Longitude));
            double dropOffLatRad = ToRadians(Convert.ToDouble(dropOffLocation.Latitude));
            double dropOffLongRad = ToRadians(Convert.ToDouble(dropOffLocation.Longitude));
            double dlong = dropOffLongRad - pickUpLongRad;
            double dlat = dropOffLatRad - pickUpLatRad;
            double ans = Math.Pow(Math.Sin(dlat / 2), 2) +
                         Math.Cos(pickUpLatRad) * Math.Cos(dropOffLatRad) *
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

        public decimal GetCommissionRate(string chargeName, List<TariffCharge> tariffCharges)
        {
            var charge = tariffCharges.FirstOrDefault(c => c.Name == chargeName);
            return charge?.Value ?? 0m;
        }

        public async Task<Driver> FindNearbyDriverAsync(int rideId)
        {
            var rideStatus = await _unitOfWork.RideRepository.GetStatus(rideId);
            var pickUpLocation = await _unitOfWork.RideRepository.GetRideLongLat(rideId);
            var allDrivers = await _unitOfWork.DriverRepository.GetAllTaxiTypeDrivers(rideId);
            var nearbyDrivers = new List<Driver>();
            int maxDistance = 5;
            foreach (var driver in allDrivers)
            {
                var driverLoc = await _unitOfWork.DriverRepository.GetLongLat(driver.UserId);
                double distance = CalculateDistance(pickUpLocation, driverLoc);

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

            return nearbyDrivers.FirstOrDefault();
        }

        public async Task<string> GetDriverAsync(int rideId)
        {
            var driver = await FindNearbyDriverAsync(rideId);
            var ride = await _unitOfWork.RideRepository.GetById(rideId);

            if (ride == null)
            {
                throw new NotFoundException(AppConstant.RideNotFound, _loggerAdapter);
            }

            if (driver == null)
            {
                return AppConstant.NodriversFound;
            }

            ride.DriverId = driver.Id;
            driver.DriverStatusId = (int)Common.Enums.DriverStatus.Unavailable;
            await _unitOfWork.DriverRepository.Update(driver);
            await _unitOfWork.RideRepository.Update(ride);
            await _unitOfWork.SaveChangesAsync();
            return AppConstant.RequestSent;
        }

        public async Task<decimal> CalculateCancellationFee(int rideId)
        {
            var tariff = await _unitOfWork.TariffChargeRepository.GetAll();
            decimal cancellationFeePercentage = tariff.FirstOrDefault(t => t.Name == AppConstant.CancellationFee)?.Value ?? 0m;
            var ridePayment = await _unitOfWork.PaymentRepository.GetByRide(rideId);
            var totalRideAmount = ridePayment.TotalFareAmount;
            return totalRideAmount * (cancellationFeePercentage / 100);
        }

        public async Task<decimal> CalculateFare(Location pickUpLocation, Location dropOffLocation)
        {
            double distance = CalculateDistance(pickUpLocation, dropOffLocation);
            var tariff = await _unitOfWork.TariffChargeRepository.GetAll();
            decimal baseFare = tariff.FirstOrDefault(t => t.Name == AppConstant.Basefare)?.Value ?? 0m;
            decimal perKmCharge = tariff.FirstOrDefault(t => t.Name == AppConstant.PerKm)?.Value ?? 0m;
            decimal fare = baseFare + (decimal)distance * perKmCharge;
            return fare;
        }

        public async Task<decimal> CalculateFareWithStop(Location pickupLocation, Location stopLocation, Location dropoffLocation)
        {
            var fareToStop = await CalculateFare(pickupLocation, stopLocation);
            var fareToEnd = await CalculateFare(stopLocation, dropoffLocation);
            decimal fare = fareToStop + fareToEnd;
            return fare;
        }
    }
}

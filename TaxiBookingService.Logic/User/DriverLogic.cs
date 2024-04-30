using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;
using AutoMapper;
using TaxiBookingService.Common.Utilities;
using Azure.Core;
using TaxiBookingService.Dal.Migrations;
using TaxiBookingService.Common.Enums;
using TaxiBookingService.Client.Interfaces;
using System.Security.Claims;

namespace TaxiBookingService.Logic.User
{
    public class DriverLogic : IDriverLogic
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IGeoCodingHttpClient _externalApiClient;
        private readonly IRideLogic _rideLogic;
        private readonly IDistanceMatrixHttpClient _distanceMatrixHttpClient;
        private readonly ILoggerAdapter _loggerAdapter;
        public DriverLogic(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IMapper mapper, IGeoCodingHttpClient externalApiClient, IRideLogic rideLogic, IDistanceMatrixHttpClient distanceMatrixHttpClient, ILoggerAdapter loggerAdapter
)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _mapper = mapper;
            _externalApiClient = externalApiClient;
            _rideLogic = rideLogic;
            _distanceMatrixHttpClient = distanceMatrixHttpClient;
            _loggerAdapter = loggerAdapter;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }

        private async Task<Driver> GetDriverFromToken()
        {
            var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var driver = await _unitOfWork.DriverRepository.GetByEmail(email.Value);

            if (driver == null)
            {
                throw new NotFoundException(AppConstant.DriverNotFound, _loggerAdapter);
            }

            return driver;
        }

        private async Task<Ride> GetExistingRide(int rideId,int driverId)
        {
            var existingRide = await _unitOfWork.RideRepository.GetById(rideId);

            if (existingRide == null || existingRide.DriverId != driverId)
            {
                throw new NotFoundException(AppConstant.RideNotFound, _loggerAdapter);
            }

            return existingRide;
        }

        private async Task<Ride> GetRide(int rideId)
        {
            var ride = await _unitOfWork.RideRepository.GetById(rideId);

            if (ride == null)
            {
                throw new NotFoundException(AppConstant.RideNotFound, _loggerAdapter);
            }

            return ride;
        }

 
        private async Task<bool> IsValidCancellationReason(string reason)
        {
            var validReasons = await _unitOfWork.RideCancellationReasonRepository.GetAllValidReasons();
            return validReasons.Any(r => string.Equals(r.Name, reason, StringComparison.OrdinalIgnoreCase));
        }

        public async Task<int> Register(DriverRegisterDto request)
        {
            var checkEmail = await _unitOfWork.UserRepository.IsEmail(request.Email);

            if (checkEmail)
            {
                throw new EmailAlreadyExistsExecption(AppConstant.EmailAlreadyExists, _loggerAdapter);
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var userEntity = _mapper.Map<Dal.Entities.User>(request);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;
            userEntity.RoleId = (int)UserRole.Driver;
            await _unitOfWork.UserRepository.Add(userEntity);
            var driverEntity = _mapper.Map<Driver>(userEntity);
            driverEntity.User = userEntity;
            driverEntity = _mapper.Map(request, driverEntity);
            await _unitOfWork.DriverRepository.Add(driverEntity);
            await _unitOfWork.SaveChangesAsync();
            return driverEntity.Id;
        }

        public async Task<int> AddTaxi(DriverTaxiDto request)
        {
            var driver = await GetDriverFromToken();
            var newTaxi = _mapper.Map<Taxi>(request);
            newTaxi.DriverId = driver.Id;
            await _unitOfWork.TaxiRepository.Add(newTaxi);
            await _unitOfWork.SaveChangesAsync();
            return newTaxi.Id;
        }

        public async Task<string> Accept(int rideId)
        {
            var driver = await GetDriverFromToken();
            var existingRide = await GetExistingRide(rideId,driver.Id);

            if (existingRide.RideStatusId != (int)Common.Enums.RideStatus.Searching)
            {
                throw new InvalidOperationException(AppConstant.DriverNotAssignedToRide);
            }

            existingRide.RideStatusId = (int)Common.Enums.RideStatus.Accepted;
            await _unitOfWork.RideRepository.Update(existingRide);
            await _unitOfWork.SaveChangesAsync();
            return AppConstant.RideAccepted;
        }

        public async Task<string> StartRide(int rideId, int verificationPin)
        {
            var existingRide = await GetRide(rideId);

            if (existingRide.RideStatusId != (int)Common.Enums.RideStatus.Accepted)
            {
                throw new InvalidOperationException(AppConstant.DriverNotAssignedToRide);
            }

            if (verificationPin != existingRide.VerificationPin)
            {
                throw new InvalidverificationPinExecption(AppConstant.InvalidverificationPin,_loggerAdapter);
            }

            existingRide.RideStatusId = (int)Common.Enums.RideStatus.Started;
            await _unitOfWork.RideRepository.Update(existingRide);
            await _unitOfWork.SaveChangesAsync();
            return AppConstant.RideStarted; 

        }

        public async Task<string> EndRide(int rideId)
        {
            var driver = await GetDriverFromToken();
            var existingRide = await GetExistingRide(rideId, driver.Id);
            var existingpayment = await _unitOfWork.PaymentRepository.GetByRide(rideId);
            var tariffCharges = await _unitOfWork.TariffChargeRepository.GetAll();

            if (existingRide.RideStatusId != (int)Common.Enums.RideStatus.Started)
            {
                throw new InvalidOperationException(AppConstant.DriverNotYetStarted);
            }

            if (existingpayment.PaymentStatusId == (int)Common.Enums.PaymentStatus.Pending)
            {
                throw new PaymentNotCompletedExecption(AppConstant.PaymentNotCompleted,_loggerAdapter);
            }

            existingRide.RideStatusId = (int)Common.Enums.RideStatus.Completed;
            existingRide.EndTime = DateTime.Now;
            await _unitOfWork.RideRepository.Update(existingRide);
            decimal commissionRate = _rideLogic.GetCommissionRate(AppConstant.DriverCommissionRate, tariffCharges.ToList());
            driver.Driverearnings += existingpayment.TotalFareAmount * (commissionRate / 100);
            driver.DriverStatusId = (int)Common.Enums.DriverStatus.Avaliable;
            await _unitOfWork.DriverRepository.Update(driver);
            await _unitOfWork.SaveChangesAsync();
            return AppConstant.RideEnded;
        }

        public async Task<string> Decline(int rideId)
        {
            var driver = await GetDriverFromToken();
            var existingRide = await GetExistingRide(rideId, driver.Id);

            if(existingRide.RideStatusId!= (int)Common.Enums.RideStatus.Searching) 
            { 
                throw new RideAlreadyAcceptedExecption(AppConstant.RideAlreadyAccepted,_loggerAdapter); 
            }

            driver.DriverStatusId = (int)Common.Enums.DriverStatus.Avaliable;
            await _unitOfWork.DriverRepository.Update(driver);
            var declineEntity = new RejectedRide { RideId = rideId, DriverId = driver.Id };
            await _unitOfWork.RejectedRideRepository.Add(declineEntity);
            await _unitOfWork.SaveChangesAsync();
            await _rideLogic.GetDriverAsync(rideId);
            return AppConstant.Declined;
        }

        public async Task<string> CancelRide(int rideId, string reason)
        {
            var driver = await GetDriverFromToken();
            var existingRide = await GetExistingRide(rideId, driver.Id);
            var rideStatus = await _unitOfWork.RideRepository.GetStatus(rideId);
            var cancellationReason = await _unitOfWork.RideCancellationReasonRepository.GetByName(reason);

            if (rideStatus == (int)Common.Enums.RideStatus.Accepted)
            {
                bool isValidReason = await IsValidCancellationReason(reason);

                if (!isValidReason)
                {
                    decimal cancellationFee = await _rideLogic.CalculateCancellationFee(rideId);
                    driver.Driverearnings -= cancellationFee;
                    await _unitOfWork.DriverRepository.Update(driver);
                }

                driver.DriverStatusId = (int)Common.Enums.DriverStatus.Avaliable;
                await _unitOfWork.DriverRepository.Update(driver);
                existingRide.RideStatusId = (int)Common.Enums.RideStatus.Searching;
                existingRide.RideCancellationReasonId = cancellationReason?.Id;
                await _unitOfWork.RideRepository.Update(existingRide);
                var declineEntity = new RejectedRide { RideId = rideId, DriverId = driver.Id };
                await _unitOfWork.RejectedRideRepository.Add(declineEntity);
                await _unitOfWork.SaveChangesAsync();
                await _rideLogic.GetDriverAsync(rideId);
            }
            else
            {
                throw new InvalidOperationException(AppConstant.CannotCancel);
            }
            return AppConstant.DriverCancelled;
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

        public async Task<string> FeedBack(DriverRatingDto request)
        {
            var ride = await _unitOfWork.RideRepository.Exists(request.RideId);
            var rideStatus = await _unitOfWork.RideRepository.GetStatus(request.RideId);

            if (!ride)
            {
                throw new NotFoundException(AppConstant.RideNotFound, _loggerAdapter);
            }

            if (rideStatus != (int)Common.Enums.RideStatus.Completed)
            {
                throw new RideNotCompletedException(AppConstant.RideNotCompleted, _loggerAdapter);
            }

            var feedback = _mapper.Map<DriverRating>(request);
            await _unitOfWork.DriverRatingRepository.Add(feedback);
            await _unitOfWork.SaveChangesAsync();
            var customerId = await _unitOfWork.RideRepository.GetCustomerByRideId(request.RideId);
            await UpdateCustomerRating(customerId);
            return AppConstant.Feedback;
        }

        public async Task<List<DriverRideDisplayDto>> RideHistory()
        {
            var driver = await GetDriverFromToken();
            var rides = await _unitOfWork.RideRepository.GetAllDriverRides(driver.Id);

            if (rides.Count == 0)
            {
                throw new NotFoundException(AppConstant.Norides, _loggerAdapter);
            }

            var rideHistoryDtoList = new List<DriverRideDisplayDto>();
            foreach (var ride in rides)
            {
                var pickUpAddress = await _externalApiClient.GetReverseGeocodingAsync(ride.PickupLocation.Latitude, ride.PickupLocation.Longitude);
                var dropoffAddress = await _externalApiClient.GetReverseGeocodingAsync(ride.DropoffLocation.Latitude, ride.DropoffLocation.Longitude);
                var exisitngRide=_mapper.Map<DriverRideDisplayDto>(ride);
                exisitngRide.PickupLocation = pickUpAddress;
                exisitngRide.DropoffLocation = dropoffAddress;  
                rideHistoryDtoList.Add(exisitngRide);
            }
            return rideHistoryDtoList;
        }

        public async Task<DriverGetRideDto> GetActiveRide()
        {
            var driver = await GetDriverFromToken();
            var ride = await _unitOfWork.RideRepository.GetRide(driver.Id);

            if (ride == null || ride.RideStatusId!= (int)Common.Enums.RideStatus.Searching)
            {
                throw new Exception(AppConstant.NoridesFound);
            }

            var pickUpAddress = await _externalApiClient.GetReverseGeocodingAsync(ride.PickupLocation.Latitude, ride.PickupLocation.Longitude);
            var dropoffAddress = await _externalApiClient.GetReverseGeocodingAsync(ride.DropoffLocation.Latitude, ride.DropoffLocation.Longitude);
            var eta = await _distanceMatrixHttpClient.GetDurationAsync(ride.PickupLocation, ride.DropoffLocation);
            var result=_mapper.Map<DriverGetRideDto>(ride);
            result.PickupLocation = pickUpAddress;
            result.DropoffLocation = dropoffAddress;
            result.EstimatedTimeArrival = eta;
            return result;
        }

        public async Task<string> ConfirmRidePayment(int rideId)
        {
            var existingPayment = await _unitOfWork.PaymentRepository.GetByRide(rideId);
            var existingRide = await GetRide(rideId);
            var paymentMethod = await _unitOfWork.PaymentMethodRepository.GetById(existingPayment.PaymentMethodId);

            if (existingRide.RideStatusId == (int)Common.Enums.RideStatus.Searching)
            {
                throw new NotStartedException(AppConstant.DriverNotYetStarted,_loggerAdapter);
            }
            if (paymentMethod.Name == AppConstant.Cash)
            {
                existingPayment.PaymentStatusId = (int)Common.Enums.PaymentStatus.Completed;
                await _unitOfWork.PaymentRepository.Update(existingPayment);
                await _unitOfWork.SaveChangesAsync();
                return AppConstant.PaymentSuccess;
            }
            else
            {
                return AppConstant.PaymentMadeInWallet;
            }
        }

        public async Task<Common.Enums.DriverStatus> UpdateAvailiability()
        {
            var driver =await GetDriverFromToken();
            var isDriverInRide = await _unitOfWork.RideRepository.IsDriverInRide(driver.Id);

            if (!isDriverInRide)
            {
                driver.DriverStatusId = driver.DriverStatusId == (int)Common.Enums.DriverStatus.Avaliable ?
                         (int)Common.Enums.DriverStatus.UnAvaliable :
                         (int)Common.Enums.DriverStatus.Avaliable;

                await _unitOfWork.DriverRepository.Update(driver);
                await _unitOfWork.SaveChangesAsync();
                return driver.DriverStatusId == (int)Common.Enums.DriverStatus.Avaliable ?
                    Common.Enums.DriverStatus.Avaliable :
                    Common.Enums.DriverStatus.UnAvaliable;
            }
            else
            {
                throw new CannotUpdateStatusExecption(AppConstant.CannotUpdateStatus,_loggerAdapter);
            }
        }
    }
}

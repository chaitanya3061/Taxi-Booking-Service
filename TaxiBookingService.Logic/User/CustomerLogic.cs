using AutoMapper;
using Azure.Core;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Cryptography;
using System.Text;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Client.DistanceMatrix.Interfaces;
using TaxiBookingService.Client.Geocoding.Interfaces;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
using TaxiBookingService.Dal.Migrations;
using TaxiBookingService.Logic.User.Interfaces;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Logic.User
{
    public class CustomerLogic : ICustomerLogic
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IGeoCodingHttpClient _externalApiClient;
        private readonly HttpClient _httpClient;
        private readonly IMapper _mapper;
        private readonly IRideLogic _rideLogic;
        private readonly IDistanceMatrixHttpClient _distanceMatrixHttpClient;
        private readonly ILoggerAdapter _loggerAdapter;

        public CustomerLogic(IUnitOfWork unitOfWork, IConfiguration configuration, IHttpContextAccessor httpContextAccessor, IGeoCodingHttpClient externalApiClient, IMapper mapper, HttpClient httpClient, IRideLogic rideLogic, IDistanceMatrixHttpClient distanceMatrixHttpClient, ILoggerAdapter loggerAdapter)
        {
            _unitOfWork = unitOfWork;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
            _externalApiClient = externalApiClient;
            _mapper = mapper;
            _httpClient = httpClient;
            _rideLogic = rideLogic;
            _distanceMatrixHttpClient = distanceMatrixHttpClient;
            _loggerAdapter= loggerAdapter;
        }

        private void CreatePasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
            }
        }   

        private async Task<bool> IsValidCancellationReason(string reason)
        {
            var validReasons = await _unitOfWork.RideCancellationReasonRepository.GetAllValidReasons();
            return validReasons.Any(r => string.Equals(r.Name, reason, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<Customer> GetCustomerFromToken()
        {
            var loggedInUser = _httpContextAccessor.HttpContext.Request.Cookies[AppConstant.refreshToken];
            var customer = await _unitOfWork.CustomerRepository.GetByToken(loggedInUser);
            return customer;
        }
        private int GenerateVerificationPin()
        {
            Random random = new Random();
            return random.Next(1000, 10000); 
        }

        private async Task<Ride> GetExistingRide(int rideId, int customerId)
        {
            var existingRide = await _unitOfWork.RideRepository.GetById(rideId);

            if (existingRide == null || existingRide.CustomerId != customerId)
            {
                throw new NotFoundException(AppConstant.RideNotFound, _loggerAdapter);
            }
            return existingRide;
        }

        private async Task UpdateStatus(Ride exisitngRide, Driver exisitngDriver)
        {
            exisitngRide.RideStatusId = AppConstant.Cancelled;
            await _unitOfWork.RideRepository.Update(exisitngRide);
            exisitngDriver.DriverStatusId = AppConstant.Available;
            await _unitOfWork.DriverRepository.Update(exisitngDriver);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task<Location> GetGeocoding(string dropOffLocation)
        {
            try
            {
                return await _externalApiClient.GetGeocodingAsync(dropOffLocation);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private async Task<string> GetReverseGeocoding(decimal latitude ,decimal longitude)
        {
            try
            {
                return await _externalApiClient.GetReverseGeocodingAsync(latitude, longitude);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        private void UpdateDropoffLocationCoordinates(Location existingLocation, Location newLocation)
        {
            existingLocation.Latitude = newLocation.Latitude;
            existingLocation.Longitude = newLocation.Longitude;
        }

        private async Task<decimal> CalculateAndUpdateFare(Ride existingRide, Location newDropoffLocation)
        {

            if(existingRide.PickupLocation.Latitude ==newDropoffLocation.Latitude && existingRide.PickupLocation.Longitude == newDropoffLocation.Longitude)
            {
                throw new SameLocationException(AppConstant.MismatchLocation,_loggerAdapter);
            }

            var fare = await _rideLogic.CalculateFare(existingRide.PickupLocation,newDropoffLocation);
            var existingPayment = await _unitOfWork.PaymentRepository.GetByRide(existingRide.Id);
            existingPayment.TotalFareAmount = fare;
            await _unitOfWork.PaymentRepository.Update(existingPayment);
            return fare;
        }

        public async Task<int> Register(CustomerRegisterDto request)
        {
            var checkEmail = await _unitOfWork.UserRepository.IsEmail(request.Email);

            if (checkEmail)
            {
                throw new EmailAlreadyExists(AppConstant.EmailAlreadyExists, _loggerAdapter);
            }

            CreatePasswordHash(request.Password, out byte[] passwordHash, out byte[] passwordSalt);
            var userEntity = _mapper.Map<Dal.Entities.User>(request);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;
            userEntity.RoleId = AppConstant.Customer;
            await _unitOfWork.UserRepository.Add(userEntity);
            var customerEntity = _mapper.Map<Customer>(userEntity); 
            customerEntity.User = userEntity;
            await _unitOfWork.CustomerRepository.Add(customerEntity);
            await _unitOfWork.SaveChangesAsync();
            return customerEntity.Id;
        }

        public async Task<int> BookRide(CustomerBookRideDto request)
        {

            var customer = await GetCustomerFromToken();
            var PickupLocation = await GetGeocoding(request.PickupLocation);
            var DropoffLocation = await GetGeocoding(request.DropoffLocation);
            var taxitype=await _unitOfWork.TaxiTypeRepository.GetByName(request.TaxiType);

            if (taxitype == null)
            {
                throw new NotFoundException(AppConstant.TaxiTypeNotFound,_loggerAdapter);
            }

            var fare = await _rideLogic.CalculateFare(PickupLocation,DropoffLocation);
            var result = await _unitOfWork.RideRepository.BookRide(PickupLocation, DropoffLocation, request, customer.Id);
            await _unitOfWork.PaymentRepository.CreatePayment(result, request.PaymentType, fare + customer.PenaltyFee);
            var createdRide = await _unitOfWork.RideRepository.GetById(result);
            createdRide.VerificationPin = GenerateVerificationPin();
            await _unitOfWork.RideRepository.Update(createdRide);
            await _rideLogic.GetDriverAsync(result);
            await _unitOfWork.SaveChangesAsync();
            return result;
        }

        public async Task<string> CancelRide(int rideId, string reason)
        {
            var customer = await GetCustomerFromToken();
            var driverId = await _unitOfWork.RideRepository.GetDriverByRideId(rideId);
            var driver = await _unitOfWork.DriverRepository.GetById(driverId);
            var rideStatus = await _unitOfWork.RideRepository.GetStatus(rideId);
            var existingRide = await GetExistingRide(rideId, customer.Id);

            if (rideStatus == AppConstant.Accepted)
            {
                bool isValidReason = await IsValidCancellationReason(reason);

                if (!isValidReason)
                {
                    decimal cancellationFee = await _rideLogic.CalculateCancellationFee(rideId);
                    customer.PenaltyFee += cancellationFee;
                    await _unitOfWork.CustomerRepository.Update(customer);
                }
                await UpdateStatus(existingRide, driver);
            }
            else if (rideStatus == AppConstant.Searching)
            {
                await UpdateStatus(existingRide, driver);
            }
            else
            {
                throw new CannotCancel(AppConstant.CannotCancel, _loggerAdapter);
            }

            return AppConstant.CustomerCancelled;
        }

        public async Task UpdateDriverRating(int driverId)
        {
            var ratings = await _unitOfWork.CustomerRatingRepository.GetRatingsByDriverId(driverId);

            if (ratings.Any())
            {
                float averageRating = ratings.Average(r => r.Rating).Value;
                var driver = await _unitOfWork.DriverRepository.GetById(driverId);

                if (driver != null)
                {
                    driver.DriverRating = averageRating;
                    await _unitOfWork.DriverRepository.Update(driver);
                    await _unitOfWork.SaveChangesAsync();
                }
            }
        }

        public async Task<string> FeedBack(CustomerRatingDto Feedback)
        {
            var ride = await _unitOfWork.RideRepository.Exists(Feedback.RideId);

            if (!ride)
            {
                throw new NotFoundException(AppConstant.RideNotFound, _loggerAdapter);
            }

            var feedback = _mapper.Map<CustomerRating>(Feedback);
            await _unitOfWork.CustomerRatingRepository.Add(feedback);
            var driverId = await _unitOfWork.RideRepository.GetDriverByRideId(Feedback.RideId);
            await UpdateDriverRating(driverId);
            await _unitOfWork.SaveChangesAsync();
            return AppConstant.Feedback;
        }

        public async Task<List<CustomerRideDisplayDto>> RideHistory()
        {
            var customer = await GetCustomerFromToken();
            var rides = await _unitOfWork.RideRepository.GetAllCustomerRides(customer.Id);

            if (rides.Count == 0)
            {
                throw new NotFoundException(AppConstant.Notrides, _loggerAdapter);
            }

            var rideHistoryDtoList = new List<CustomerRideDisplayDto>();
            foreach (var ride in rides)
            {
                var pickUpAddress = await GetReverseGeocoding(ride.PickupLocation.Latitude, ride.PickupLocation.Longitude);
                var dropoffAddress = await GetReverseGeocoding(ride.DropoffLocation.Latitude,ride.DropoffLocation.Longitude);
                var exisitngRide = _mapper.Map<CustomerRideDisplayDto>(ride);
                exisitngRide.PickupLocation = pickUpAddress;
                exisitngRide.DropoffLocation = dropoffAddress;
                rideHistoryDtoList.Add(exisitngRide);
            }
            return rideHistoryDtoList;
        }

        public async Task<string> UpdateDropOffLocation(CustomerUpdateDropOffDto request)
        {
            var newDropoffLocation = await GetGeocoding(request.DropOffLocation);
            var existingRide = await _unitOfWork.RideRepository.GetById(request.RideId);
            var existingPayment = await _unitOfWork.PaymentRepository.GetByRide(request.RideId);

            if (existingRide.Id == AppConstant.RideCompleted)
            {
                throw new CannotUpdateDropOff(AppConstant.CannotUpdateDropOff,_loggerAdapter);
            }

            var existingDropoffLocation = existingRide.DropoffLocation;
            UpdateDropoffLocationCoordinates(existingDropoffLocation, newDropoffLocation);
            await _unitOfWork.LocationRepository.Update(existingDropoffLocation);
            var fare = await CalculateAndUpdateFare(existingRide, newDropoffLocation);
            existingPayment.TotalFareAmount = fare;
            await _unitOfWork.PaymentRepository.Update(existingPayment);
            await _unitOfWork.SaveChangesAsync();
            return $"Updated drop-off location: {request.DropOffLocation} (Fare: {fare})";
        }

        public async Task<string> TopUpWallet(int amount)
        {
            var customer = await GetCustomerFromToken();
            customer.Customerwallet = amount;
            await _unitOfWork.CustomerRepository.Update(customer);
            return AppConstant.AmountAdded;
        }

        public async Task<string> AddTrustedContact(CustomerTrustedContactDto request)
        {
            var trustedentity = _mapper.Map<TrustedContacts>(request);
            var customer = await GetCustomerFromToken();
            trustedentity.CustomerId = customer.Id;
            await _unitOfWork.TrustedContactRepository.Add(trustedentity);
            await _unitOfWork.SaveChangesAsync();
            return AppConstant.AddedContacts;
        }

        public async Task<DriverDisplayDto> GetMatchedDriver(int rideId)
        {
            var customer = await GetCustomerFromToken();
            var existingRide = await GetExistingRide(rideId, customer.Id);

            if (existingRide.RideStatusId == AppConstant.Searching)
            {
                throw new NomatchesFound(AppConstant.Nomatching,_loggerAdapter);
            }

            var user = await _unitOfWork.UserRepository.GetByDriverId(existingRide.DriverId.Value);
            var eta = await _distanceMatrixHttpClient.GetDurationAsync(existingRide.PickupLocation,existingRide.DropoffLocation);
            var result = _mapper.Map<DriverDisplayDto>(user);
            result.VerificationPin=existingRide.VerificationPin;
            result.EstimatedTimeArrival = eta;
            return result;
        }

        public async Task<string> MakePayment(int rideId)
        {
            var customer = await GetCustomerFromToken();
            var existingRide = await GetExistingRide(rideId, customer.Id);
            var existingPayment = await _unitOfWork.PaymentRepository.GetByRide(rideId);
            var paymentMethod = existingPayment.PaymentMethod;
            if (paymentMethod.Name == AppConstant.Wallet)
            {
                if (existingRide.RideStatusId == AppConstant.Started|| existingRide.RideStatusId == AppConstant.RideCompleted)
                {
                    var estimatedFare = existingPayment.TotalFareAmount;
                    customer.Customerwallet -= estimatedFare;
                    customer.PenaltyFee = 0;
                    existingPayment.PaymentStatusId = AppConstant.Completed;
                    await _unitOfWork.CustomerRepository.Update(customer);
                    await _unitOfWork.PaymentRepository.Update(existingPayment);
                    await _unitOfWork.SaveChangesAsync();
                }
                else
                {
                    throw new NotStarted(AppConstant.RideNotStarted, _loggerAdapter);
                }
                return AppConstant.PaymentSuccess;
            }
            else
            {
                return AppConstant.PaymentMadeInCash;

            }
        }

        public async Task<string> AddStop(CustomerAddStopDto request)
        {
            var customer = await GetCustomerFromToken();
            var existingRide = await GetExistingRide(request.rideId, customer.Id);
            var exisitingPayment = existingRide.Payment;

            if (existingRide.RideStatusId == AppConstant.RideCompleted)
            {
                throw new InvalidOperationException(AppConstant.CannotAddStopLocation);
            }

            var stopLocation = await GetGeocoding(request.Stop1Location);
            var newFare = await _rideLogic.CalculateFareWithStop(existingRide.PickupLocation, stopLocation, existingRide.DropoffLocation);
            existingRide.Stop1Location = stopLocation;
            exisitingPayment.TotalFareAmount= newFare;
            await _unitOfWork.RideRepository.Update(existingRide);
            await _unitOfWork.PaymentRepository.Update(exisitingPayment);
            await _unitOfWork.SaveChangesAsync();
            return AppConstant.StopLocationAdded;
        }

        public async Task<string> DeleteStop(int rideId)
        {
            var customer = await GetCustomerFromToken();
            var existingRide = await GetExistingRide(rideId, customer.Id);
            var exisitingPayment = existingRide.Payment;

            if (existingRide.RideStatusId == AppConstant.RideCompleted)
            {
                throw new InvalidOperationException(AppConstant.CannotDeleteStopLocation);
            }

            var newFare = await _rideLogic.CalculateFare(existingRide.PickupLocation,existingRide.DropoffLocation);
            existingRide.StopId1 = null;
            exisitingPayment.TotalFareAmount = newFare;
            await _unitOfWork.RideRepository.Update(existingRide);
            await _unitOfWork.PaymentRepository.Update(exisitingPayment);
            await _unitOfWork.SaveChangesAsync();
            return AppConstant.StopLocationDelete;
        }
    }
}

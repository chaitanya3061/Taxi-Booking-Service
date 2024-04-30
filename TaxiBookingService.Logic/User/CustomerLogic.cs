using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Client.Interfaces;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Enums;
using TaxiBookingService.Common.Utilities;
using TaxiBookingService.Dal.Entities;
using TaxiBookingService.Dal.Interfaces;
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

        private async Task<bool> IsValidCancellationReason(string reason)
        {
            var validReasons = await _unitOfWork.RideCancellationReasonRepository.GetAllValidReasons();
            return validReasons.Any(r => string.Equals(r.Name, reason, StringComparison.OrdinalIgnoreCase));
        }

        private async Task<Customer> GetCustomerFromToken()
        {
            var email = _httpContextAccessor.HttpContext.User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
            var customer = await _unitOfWork.CustomerRepository.GetByEmail(email.Value);

            if (customer == null)
            {
                throw new NotFoundException(AppConstant.CustomerNotFound, _loggerAdapter);
            }

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

        private async Task UpdateStatus(Ride exisitngRide, Driver exisitngDriver, int? cancellationReasonId)
        {
            exisitngRide.RideStatusId = (int)Common.Enums.RideStatus.Cancelled;
            exisitngRide.RideCancellationReasonId = cancellationReasonId;
            await _unitOfWork.RideRepository.Update(exisitngRide);
            exisitngDriver.DriverStatusId = (int)Common.Enums.DriverStatus.Available;
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
                throw new SameLocationException(AppConstant.SameLocation,_loggerAdapter);
            }

            var fare = await _rideLogic.CalculateFare(existingRide.PickupLocation,newDropoffLocation);
            var existingPayment = await _unitOfWork.PaymentRepository.GetByRide(existingRide.Id);
            existingPayment.TotalFareAmount = fare;
            await _unitOfWork.PaymentRepository.Update(existingPayment);
            return fare;
        }

        private async Task ValidateEmail(string email)
        {
            var emailExists = await _unitOfWork.UserRepository.IsEmail(email);
            if (emailExists)
            {
                throw new EmailAlreadyExistsExecption(AppConstant.EmailAlreadyExists, _loggerAdapter);
            }
        }

        private (byte[] passwordHash, byte[] passwordSalt) CreatePasswordHash(string password)
        {
            using (var hmac = new HMACSHA512())
            {
                var passwordSalt = hmac.Key;
                var passwordHash = hmac.ComputeHash(Encoding.UTF8.GetBytes(password));
                return (passwordHash, passwordSalt);
            }
        }

        private Dal.Entities.User CreateUserEntity(CustomerRegisterDto request, byte[] passwordHash, byte[] passwordSalt)
        {
            var userEntity = _mapper.Map<Dal.Entities.User>(request);
            userEntity.PasswordHash = passwordHash;
            userEntity.PasswordSalt = passwordSalt;
            userEntity.RoleId = (int)UserRole.Customer;
            return userEntity;
        }

        private Customer CreateCustomerEntity(Dal.Entities.User userEntity)
        {
            var customerEntity = _mapper.Map<Customer>(userEntity);
            customerEntity.User = userEntity;
            return customerEntity;
        }

        private async Task ValidateBooking(CustomerBookRideDto request, Customer customer, decimal fare)
        {
            var isCustomerInSearch = await _unitOfWork.RideRepository.HasActiveRideRequest(customer.Id);
            if (isCustomerInSearch)
            {
                throw new CustomerAlreadyInSearchRideException(AppConstant.CustomerAlreadyInSearchRide, _loggerAdapter);
            }

            var taxiType = await _unitOfWork.TaxiTypeRepository.GetByName(request.TaxiType);

            if (taxiType == null)
            {
                throw new NotFoundException(AppConstant.TaxiTypeNotFound, _loggerAdapter);
            }

            if (request.PaymentType.ToLower() == Common.Enums.PaymentMethod.Wallet.ToString().ToLower() && customer.Customerwallet < fare)
            {
                throw new InsufficientFundsException(AppConstant.Insufficientfunds, _loggerAdapter);
            }
        }

        private async Task CreatePaymentForRide(int rideId, string paymentType, decimal totalAmount)
        {
            await _unitOfWork.PaymentRepository.CreatePayment(rideId, paymentType, totalAmount);
        }

        private async Task<Ride> UpdateRideVerificationPin(int rideId)
        {
            var createdRide = await _unitOfWork.RideRepository.GetById(rideId);
            createdRide.VerificationPin = GenerateVerificationPin();
            await _unitOfWork.RideRepository.Update(createdRide);
            return createdRide;
        }

        private async Task ApplyCancellationFee(Customer customer, Ride ride)
        {
            decimal cancellationFee = await _rideLogic.CalculateCancellationFee(ride.Id);
            customer.PenaltyFee += cancellationFee;
            await _unitOfWork.CustomerRepository.Update(customer);
        }

        private async Task UpdateDriverRating(int driverId)
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

        private async Task ValidateFeedbackRequest(CustomerRatingDto request)
        {
            var rideExists = await _unitOfWork.RideRepository.Exists(request.RideId);
            if (!rideExists)
            {
                throw new NotFoundException(AppConstant.RideNotFound, _loggerAdapter);
            }

            var rideStatus = await _unitOfWork.RideRepository.GetStatus(request.RideId);
            if (rideStatus != (int)Common.Enums.RideStatus.Completed)
            {
                throw new RideNotCompletedException(AppConstant.RideNotCompleted, _loggerAdapter);
            }
        }

        private async Task<List<CustomerRideDisplayDto>> MapRidesToDtoList(List<Ride> rides)
        {
            var rideHistoryDtoList = new List<CustomerRideDisplayDto>();

            foreach (var ride in rides)
            {
                var pickUpAddress = await GetReverseGeocoding(ride.PickupLocation.Latitude, ride.PickupLocation.Longitude);
                var dropoffAddress = await GetReverseGeocoding(ride.DropoffLocation.Latitude, ride.DropoffLocation.Longitude);
                var exisitngRide = _mapper.Map<CustomerRideDisplayDto>(ride);
                exisitngRide.PickupLocation = pickUpAddress;
                exisitngRide.DropoffLocation = dropoffAddress;
                rideHistoryDtoList.Add(exisitngRide);
            }

            return rideHistoryDtoList;
        }

        private async Task<Payment> GetExistingPayment(Ride existingRide)
        {
            var existingPayment = await _unitOfWork.PaymentRepository.GetByRide(existingRide.Id);

            if (existingPayment == null)
            {
                throw new NotFoundException(AppConstant.PaymentNotFound, _loggerAdapter);
            }
            return existingPayment;
        }

        private async Task UpdateCustomerAndPayment(Customer customer, Payment existingPayment, decimal estimatedFare)
        {
            customer.Customerwallet -= estimatedFare;
            customer.PenaltyFee = 0;
            existingPayment.PaymentStatusId = (int)Common.Enums.PaymentStatus.Completed;

            await _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.PaymentRepository.Update(existingPayment);
            await _unitOfWork.SaveChangesAsync();
        }

        private async Task UpdateRideAndPayment(Ride existingRide, decimal newFare, Location stopLocation)
        {
            existingRide.Stop1Location = stopLocation;
            existingRide.Payment.TotalFareAmount = newFare;

            await _unitOfWork.RideRepository.Update(existingRide);
            await _unitOfWork.PaymentRepository.Update(existingRide.Payment);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<int> Register(CustomerRegisterDto request)
        {
            await ValidateEmail(request.Email);

            var (passwordHash, passwordSalt) = CreatePasswordHash(request.Password);

            var userEntity = CreateUserEntity(request, passwordHash, passwordSalt);
            await _unitOfWork.UserRepository.Add(userEntity);

            var customerEntity = CreateCustomerEntity(userEntity);
            await _unitOfWork.CustomerRepository.Add(customerEntity);

            await _unitOfWork.SaveChangesAsync();

            return customerEntity.Id;
        }


        public async Task<int> BookRide(CustomerBookRideDto request)
        {
            var customer = await GetCustomerFromToken();
            var pickupLocation = await GetGeocoding(request.PickupLocation);
            var dropoffLocation = await GetGeocoding(request.DropoffLocation);
            var fare = await _rideLogic.CalculateFare(pickupLocation, dropoffLocation);

            await ValidateBooking(request, customer, fare);

            var rideId = await _unitOfWork.RideRepository.BookRide(pickupLocation, dropoffLocation, request, customer.Id);

            await CreatePaymentForRide(rideId, request.PaymentType, fare + customer.PenaltyFee);

            var createdRide = await UpdateRideVerificationPin(rideId);

            await _rideLogic.GetDriverAsync(rideId);

            await _unitOfWork.SaveChangesAsync();

            return rideId;
        }

        public async Task<string> CancelRide(int rideId, string reason)
        {
            var customer = await GetCustomerFromToken();
            var driverId = await _unitOfWork.RideRepository.GetDriverByRideId(rideId);
            var driver = await _unitOfWork.DriverRepository.GetById(driverId);
            var existingRide = await GetExistingRide(rideId, customer.Id);
            var cancellationReason = await _unitOfWork.RideCancellationReasonRepository.GetByName(reason);

            if (existingRide.RideStatusId == (int)Common.Enums.RideStatus.Completed || existingRide.RideStatusId == (int)Common.Enums.RideStatus.Started)
            {
                throw new CannotCancelException(AppConstant.CannotCancel, _loggerAdapter);
            }

            if (existingRide.RideStatusId == (int)Common.Enums.RideStatus.Accepted)
            {
                bool isValidReason = await IsValidCancellationReason(reason);

                if (!isValidReason)
                {
                    await ApplyCancellationFee(customer, existingRide);
                }
            }

            await UpdateStatus(existingRide, driver, cancellationReason?.Id);

            return AppConstant.CustomerCancelled;
        }

        public async Task<string> FeedBack(CustomerRatingDto request)
        {
            await ValidateFeedbackRequest(request);

            var feedback = _mapper.Map<CustomerRating>(request);

            await _unitOfWork.CustomerRatingRepository.Add(feedback);

            var driverId = await _unitOfWork.RideRepository.GetDriverByRideId(request.RideId);
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
                throw new NotFoundException(AppConstant.Norides, _loggerAdapter);
            }

            var rideHistoryDtoList = await MapRidesToDtoList(rides);

            return rideHistoryDtoList;

        }

        public async Task<string> UpdateDropOffLocation(CustomerUpdateDropOffDto request)
        {
            var newDropoffLocation = await GetGeocoding(request.DropOffLocation);
            var customer = await GetCustomerFromToken();
            var existingRide = await GetExistingRide(request.RideId, customer.Id);

            var existingPayment = await GetExistingPayment(existingRide);

            if (existingRide.Id == (int)Common.Enums.RideStatus.Completed)
            {
                throw new CannotUpdateDropOffExecption(AppConstant.CannotUpdateDropOff,_loggerAdapter);
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
            if (amount < 0) 
            {
                throw new InvalidTopUpAmountException(AppConstant.InvalidTopUpAmount,_loggerAdapter);
            }

            var customer = await GetCustomerFromToken();
            customer.Customerwallet+= amount;
            await _unitOfWork.CustomerRepository.Update(customer);
            await _unitOfWork.SaveChangesAsync();

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

        public async Task<DriverDisplayDto> GetAllocatedDriver(int rideId)
        {
            var customer = await GetCustomerFromToken();
            var existingRide = await GetExistingRide(rideId, customer.Id);

            if (existingRide.RideStatusId == (int)Common.Enums.RideStatus.Searching)
            {
                throw new Exception(AppConstant.DriverNotAllocated);
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
            var existingPayment = await GetExistingPayment(existingRide);
            var paymentMethod = existingPayment.PaymentMethod;

            if (paymentMethod.Name == AppConstant.Wallet)
            {
                if (existingRide.RideStatusId == (int)Common.Enums.RideStatus.Started || existingRide.RideStatusId == (int)Common.Enums.RideStatus.Completed)
                {
                    var estimatedFare = existingPayment.TotalFareAmount;

                    await UpdateCustomerAndPayment(customer, existingPayment, estimatedFare);
                }
                else
                {
                    throw new NotStartedException(AppConstant.RideNotStarted, _loggerAdapter);
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

            if (existingRide.RideStatusId == (int)Common.Enums.RideStatus.Completed)
            {
                throw new InvalidOperationException(AppConstant.CannotAddStopLocation);
            }

            var stopLocation = await GetGeocoding(request.Stop1Location);
            var newFare = await _rideLogic.CalculateFareWithStop(existingRide.PickupLocation, stopLocation, existingRide.DropoffLocation);

            await UpdateRideAndPayment(existingRide, newFare,stopLocation);

            return AppConstant.StopLocationAdded;
        }

        public async Task<string> DeleteStop(int rideId)
        {
            var customer = await GetCustomerFromToken();
            var existingRide = await GetExistingRide(rideId, customer.Id);
            var exisitingPayment = existingRide.Payment;

            if (existingRide.RideStatusId == (int)Common.Enums.RideStatus.Completed)
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

        public async Task<int> ScheduleRide(CustomerBookRideDto request)
        {

            var customer = await GetCustomerFromToken();
            var PickupLocation = await GetGeocoding(request.PickupLocation);
            var DropoffLocation = await GetGeocoding(request.DropoffLocation);

            var fare = await _rideLogic.CalculateFare(PickupLocation, DropoffLocation);

            await ValidateBooking(request, customer, fare);

            var rideId = await _unitOfWork.RideRepository.BookRide(PickupLocation, DropoffLocation, request, customer.Id);

            await CreatePaymentForRide(rideId, request.PaymentType, fare + customer.PenaltyFee);

            if (!request.ScheduledDate.HasValue) 
            {
                throw new ScheduleDateNotProvidedException(AppConstant.ScheduleDate,_loggerAdapter);
            }

            await _unitOfWork.ScheduleRideRepository.CreateScheduleRide(rideId, request.ScheduledDate.Value);

            var createdRide = await UpdateRideVerificationPin(rideId);

            await _unitOfWork.SaveChangesAsync();
            return rideId;
        }

        public async Task<List<ScheduledRide>> GetAllActiveSchedulerides()
        {
            var result=await _unitOfWork.ScheduleRideRepository.GetAllActiveSchedulerides();
            return result;
        }
    }
}

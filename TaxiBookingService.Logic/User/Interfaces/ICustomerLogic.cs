using TaxiBookingService.API.User.Customer;
using TaxiBookingService.API.User.Driver;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface ICustomerLogic
    {
        Task<int> Register(CustomerRegisterDto request);
        Task<int> BookRide(CustomerBookRideDto request);
        Task<int> ScheduleRide(CustomerBookRideDto request);
        Task<string> CancelRide(int rideId, string reason);
        Task<string> FeedBack(CustomerRatingDto rating);
        Task<List<CustomerRideDisplayDto>> RideHistory();
        Task<string> UpdateDropOffLocation(CustomerUpdateDropOffDto request);
        Task<string> TopUpWallet(int amount);
        Task<string> AddTrustedContact(CustomerTrustedContactDto request);
        Task<DriverDisplayDto> GetAllocatedDriver(int rideId);
        Task<string> MakePayment(int rideId);
        Task<string>AddStop(CustomerAddStopDto request);
        Task<string>DeleteStop(int rideId);
        Task<List<ScheduledRide>> GetAllActiveSchedulerides();
    }
}

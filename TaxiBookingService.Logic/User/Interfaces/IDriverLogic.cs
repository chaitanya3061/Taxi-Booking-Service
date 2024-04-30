using TaxiBookingService.API.User.Driver;

namespace TaxiBookingService.Logic.User.Interfaces
{
    public interface IDriverLogic
    {
        Task<int> Register(DriverRegisterDto request);
        Task<int> AddTaxi(DriverTaxiDto taxi);
        Task<string>Accept(int rideId);
        Task<string> Decline(int rideId);
        Task<string> StartRide(int rideId, int verificationPin);
        Task<string> EndRide(int rideId);
        Task<string> CancelRide(int rideId,string reason);   
        Task<string> FeedBack(DriverRatingDto rating);
        Task<List<DriverRideDisplayDto>> RideHistory();
        Task<DriverGetRideDto> GetActiveRide();
        Task<string> ConfirmRidePayment(int  rideId);
        Task<Common.Enums.DriverStatus> UpdateAvailiability();
    }
}

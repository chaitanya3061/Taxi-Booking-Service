
namespace TaxiBookingService.API.User.Driver
{
    public class DriverGetRideDto
    {
        public int RideId { get; set; }
        public string TaxiType { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public string EstimatedTimeArrival { get; set; }

    }
}

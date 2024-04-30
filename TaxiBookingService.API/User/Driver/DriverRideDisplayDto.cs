
namespace TaxiBookingService.API.User.Driver
{
    public class DriverRideDisplayDto
    {
        public int Id { get; set; }
        public string Customer { get; set; }
        public string TaxiType { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Amount { get; set; }
        public String EstimatedTimeArrival {  get; set; }
    }
}

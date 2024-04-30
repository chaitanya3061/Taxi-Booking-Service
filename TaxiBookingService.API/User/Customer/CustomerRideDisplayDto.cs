
namespace TaxiBookingService.API.User.Customer
{
    public class CustomerRideDisplayDto
    {
        public int Id { get; set; }
        public string Driver { get; set; }
        public string TaxiType { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public decimal Amount { get; set; }
    }
}

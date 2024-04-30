
namespace TaxiBookingService.API.Ride
{
    public class RideDisplayDto
    {
        public string Customer { get; set; }
        public string Driver { get; set; }
        public string TaxiType { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public Decimal TotalFare { get; set; }
    }
}

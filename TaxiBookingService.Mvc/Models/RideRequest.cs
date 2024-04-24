namespace TaxiBookingService.Mvc.Models
{
    public class RideRequest
    {
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public string TaxiType { get; set; }
    }
}

using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerAddStopDto
    {
        [PositiveIntegerValidation]
        public int rideId { get; set; }

        [LocationValidation]
        public string Stop1Location { get; set; }
        public string Stop2Location { get; set; }
    }
}

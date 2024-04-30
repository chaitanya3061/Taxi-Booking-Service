using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerUpdateDropOffDto
    {
        [PositiveIntegerValidation]
        public int RideId { get; set; }

        [LocationValidation]
        public string DropOffLocation { get; set; }
    }
}

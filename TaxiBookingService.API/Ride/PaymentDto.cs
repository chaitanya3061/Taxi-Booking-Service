using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.Ride
{
    public class PaymentDto
    {
        [PositiveIntegerValidation]
        public int CustomerId { get; set; }

        [PositiveIntegerValidation]
        public int RideId { get; set; }

        [RequiredValidation]
        public string PaymentType { get; set; }

    }
}

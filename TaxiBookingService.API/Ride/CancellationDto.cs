using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.Ride
{
    public class CancellationDto
    {
        [PositiveIntegerValidation]
        public int RideId { get; set; }


        [RequiredValidation]
        public string Reason { get; set; }
    }
}

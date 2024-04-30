using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerBookRideDto
    {
        [LocationValidation]
        public string PickupLocation { get; set; }

        [LocationValidation]
        [DifferentLocation(nameof(PickupLocation), ErrorMessage = AppConstant.SameLocation)]
        public string DropoffLocation { get; set; }

        [RequiredValidation]
        public string TaxiType { get; set; }

        [RequiredValidation]
        public string PaymentType { get; set; }

        [DateTimeValidation]
        public DateTime? ScheduledDate { get; set; }
    }
}

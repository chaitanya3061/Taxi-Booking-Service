using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerScheduleRideDTO
    {

        [LocationValidation]
        public string PickupLocation { get; set; }

        [LocationValidation]
        [Compare(nameof(PickupLocation), ErrorMessage = AppConstant.MismatchLocation)]
        public string DropoffLocation { get; set; }

        [RequiredValidation]
        public string TaxiType { get; set; }

        [RequiredValidation]
        public string PaymentType { get; set; }

        [DateTimeValidation]
        public string ScduleDate { get; set; }
    }
}

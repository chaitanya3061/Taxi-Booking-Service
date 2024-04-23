using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerBookRideDto
    {
        [LocationValidationAttribute]
        public string PickupLocation { get; set; }

        [LocationValidationAttribute]
        [Compare(nameof(PickupLocation), ErrorMessage = "locations mismatch")]
        public string DropoffLocation { get; set; }

        [RequiredValidationAttribute]
        public string TaxiType { get; set; }

        [RequiredValidationAttribute]
        public string PaymentType { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerTrustedContactDto
    {
        
        [RequiredValidation]
        public string ContactName { get; set; }

        [RequiredValidation]
        public string CountryCode { get; set; }

        [PhoneNumberValidation]
        public string ContactPhoneNumber { get; set; }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attribute;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Driver
{
    public class DriverRegisterDto
    {
        [RequiredValidationAttribute]
        public string Name { get; set; }

        [EmailValidationAttribute]
        public string Email { get; set; }


        [RequiredValidationAttribute]
        public string Password { get; set; }

        [RequiredValidationAttribute]
        public string CountryCode { get; set; }


        [PhoneNumberValidation]
        public string PhoneNumber { get; set; }


        [LicenceNumberValidation]
        public string LicenseNumber { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Driver
{
    public class DriverTaxiDto
    {
        [RequiredValidationAttribute]
        public string Name { get; set; }


        [RegistrationNumberAttribute]
        public string RegistrationNumber { get; set; }

        [RequiredValidationAttribute]
        public string Color { get; set; }

        [RequiredValidationAttribute]
        public string TaxiType { get; set; } 
    }
}

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
        [RequiredValidation]
        public string Name { get; set; }

        [RegistrationNumber]
        public string RegistrationNumber { get; set; }

        public string Color { get; set; }

        [RequiredValidation]
        public string TaxiType { get; set; } 
    }
}

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
        public string Name { get; set; }

        public string RegistrationNumber { get; set; }

        public string Color { get; set; }

        public int TaxiTypeId { get; set; } 
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.API.User.Driver
{
    public class DriverTaxiServiceContracts
    {
        public string Name { get; set; }
        public string RegistrationNumber { get; set; }
        public string Color { get; set; }
        public string Model { get; set; }
        public string TaxiType { get; set; } //dtos
    }
}

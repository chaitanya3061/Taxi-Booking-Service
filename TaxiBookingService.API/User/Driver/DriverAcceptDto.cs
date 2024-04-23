using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Driver
{
    public class DriverAcceptDto
    {
        [PositiveIntegerValidation]
        public int RideId { get; set; }

        [PositiveIntegerValidation]
        public int DriverId { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerAddStopDto
    {
        [PositiveIntegerValidation]
        public int rideId { get; set; }

        [LocationValidation]
        public string Stop1Location { get; set; }
        public string Stop2Location { get; set; }
    }
}

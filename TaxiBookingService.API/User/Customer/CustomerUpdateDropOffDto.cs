using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerUpdateDropOffDto
    {
        [PositiveIntegerValidation]
        public int RideId { get; set; }

        [LocationValidation]
        public string DropOffLocation { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.Ride
{
    public class PaymentDto
    {
        [PositiveIntegerValidation]
        public int CustomerId { get; set; }

        [PositiveIntegerValidation]
        public int RideId { get; set; }

        [RequiredValidation]
        public string PaymentType { get; set; }

    }
}

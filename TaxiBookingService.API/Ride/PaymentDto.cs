using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.API.Ride
{
    public class PaymentDto
    {
        public int PaymentId { get; set; }
        public int CustomerId { get; set; }
        public int DriverId { get; set; }
        public int RideId { get; set; }
        public decimal Amount { get; set; }
        public DateTime PaymentDate { get; set; }

    }
}

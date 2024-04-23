using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.API.Ride
{
    public class RideHistoryDto
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public int DriverId { get; set; }
        public string TaxiType { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public decimal Amount { get; set; }
    }
}

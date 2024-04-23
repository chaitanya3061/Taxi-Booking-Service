using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.Ride
{
    public class RideDisplayDto
    {
        public string TaxiType { get; set; }
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public Decimal TotalFare { get; set; }
    }
}

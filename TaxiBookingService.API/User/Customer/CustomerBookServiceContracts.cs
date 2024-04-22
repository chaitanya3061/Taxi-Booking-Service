using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerBookServiceContracts
    {
        public string PickupLocation { get; set; }
        public string DropoffLocation { get; set; }
        public string TaxiType { get; set; }
        public string PaymentType { get; set; }
        public string ScheduledDate { get; set; }                           

    }
}

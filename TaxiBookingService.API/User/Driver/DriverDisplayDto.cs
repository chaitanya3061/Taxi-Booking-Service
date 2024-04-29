using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.API.User.Driver
{
    public class DriverDisplayDto
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public float DriverRating { get; set; }
        public string EstimatedTimeArrival { get; set; }
        public int VerificationPin {  get; set; }
    }
}

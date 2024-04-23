using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Driver
{
    public class DriverRatingDto
    {
        [PositiveIntegerValidation]
        public int RideId { get; set; }

        public string Feedback { get; set; }

        public float Rating { get; set; }
    }
}

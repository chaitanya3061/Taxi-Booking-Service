using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Entities
{
    public class RejectedRide
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        [ForeignKey("Ride")]
        public int RideId { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual Ride Ride { get; set; }
    }
}

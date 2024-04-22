using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using TaxiBookingService.Common.Enums;

namespace TaxiBookingService.Dal.Entities
{
    public class Driver
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(16)")]

        public string LicenceNumber { get; set; }

        [Column(TypeName = "decimal(18,2)")]

        public decimal Driverearnings { get; set; }

        [Column(TypeName = "float")]
        public float? DriverRating { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }


        public bool IsActive { get; set; }

        [ForeignKey("DriverStatus")]
        public int DriverStatusId { get; set; }

        public virtual User User { get; set; }

        public virtual DriverStatus DriverStatus { get; set; }
        public ICollection<Ride> Rides { get; set; }
        public ICollection<RejectedRide> RejectedRides { get; set; }

    }
}

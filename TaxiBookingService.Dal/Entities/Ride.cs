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
    public class Ride
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; }

        [ForeignKey("Driver")]
        public int? DriverId { get; set; }

        [ForeignKey("TaxiType")]
        public int TaxiTypeId { get; set; }

        [ForeignKey("PickupLocation")]
        public int PickupLocationId { get; set; }

        [ForeignKey("DropoffLocation")]
        public int DropoffLocationId { get; set; }

        [ForeignKey("Stop1Location")]
        public int? StopId1 { get; set; }

        [ForeignKey("Stop2Location")]
        public int? StopId2 { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime StartTime { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime EndTime { get; set; }

        [ForeignKey("RideStatus")]
        public int RideStatusId { get; set; }

        [ForeignKey("RideCancellationReason")]
        public int? RideCancellationReasonId { get; set; }

        [Column(TypeName = "datetime2")]

        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedAt { get; set; }

        public virtual Driver Driver { get; set; }
        public virtual Customer Customer { get; set; }
        public virtual TaxiType TaxiType { get; set; }
        public virtual Location PickupLocation { get; set; } 
        public virtual Location DropoffLocation { get; set; }
        public virtual Location Stop1Location { get; set; }
        public virtual Location Stop2Location { get; set; }
        public virtual RideStatus Ridestatus { get; set; }
        public virtual RideCancellationReason RideCancellationReason { get; set; }
        public virtual ICollection<RejectedRide> RejectedRides { get; set; }

    }
}

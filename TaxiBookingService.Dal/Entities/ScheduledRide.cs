using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiBookingService.Dal.Entities
{
    public class ScheduledRide
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ride")]
        public int RideId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime ScheduledDate  { get; set; }

        public virtual Ride Ride { get; set; }
    }
}

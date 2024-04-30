using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace TaxiBookingService.Dal.Entities
{
    public class CustomerRating
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ride")]
        public int RideId { get; set; }

        [Column(TypeName = "varchar(50)")]
        public string? Feedback { get; set; }

        public float? Rating { get; set; }

        public virtual Ride Ride { get; set; }
    }
}


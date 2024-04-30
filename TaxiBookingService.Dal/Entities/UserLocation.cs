using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TaxiBookingService.Dal.Entities
{
    public class UserLocation
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column(TypeName = "decimal(12, 6)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(12, 6)")]
        public decimal Longitude { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime TimeStamp { get; set; }

        public virtual User User { get; set; }
    }
}

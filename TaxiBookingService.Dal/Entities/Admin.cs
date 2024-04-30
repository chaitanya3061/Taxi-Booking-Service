using System.ComponentModel.DataAnnotations.Schema;

namespace TaxiBookingService.Dal.Entities
{
    public class Admin
    {
        public int Id { get; set; }

        [ForeignKey("User")]
        public int UserId { get; set; }

        public virtual User User { get; set; }
    }
}

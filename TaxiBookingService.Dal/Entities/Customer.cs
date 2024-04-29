using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Entities
{
    public class Customer 
    {
        [Key]
        public int Id { get; set; }
        [ForeignKey("User")]
        public int UserId { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Customerwallet { get; set; }

        [Column(TypeName = "float")]
        public float? CustomerRating { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal PenaltyFee { get; set; }

        public virtual User User { get; set; }

        public ICollection<Ride> Rides { get; set; }

        public ICollection<TrustedContacts> TrustedContacts { get; set; }

    }
}

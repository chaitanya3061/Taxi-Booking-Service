using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Entities
{
    public class TrustedContacts
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Customer")]
        public int CustomerId { get; set; } 

        [Column(TypeName = "varchar(5)")]
        public string CountryCode { get; set; }

        [Column(TypeName = "varchar(25)")]
        public string ContactName { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string ContactPhoneNumber { get; set; }

        public bool IsDeleted { get; set; }

        public virtual Customer Customer { get; set; }
    }
}

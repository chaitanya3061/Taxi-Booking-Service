using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Entities
{
    public class Taxi
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Driver")]
        public int DriverId { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }

        
        [Column(TypeName = "varchar(13)")]
        public string RegistrationNumber { get; set; }

        [Column(TypeName = "varchar(10)")]
        public string? Color { get; set; }

        [ForeignKey("TaxiType")]
        public int TaxiTypeId { get; set; }

        [Column(TypeName = "datetime2")]
        
        public DateTime CreatedAt { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime? ModifiedAt { get; set; }

        
        public bool IsDeleted { get; set; }

        public Driver Driver { get; set; }
        public virtual TaxiType TaxiType { get; set; }

    }
}

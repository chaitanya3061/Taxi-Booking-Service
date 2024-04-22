using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Entities
{
    public class Location
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "decimal(12, 6)")]
        public decimal Latitude { get; set; }

        [Column(TypeName = "decimal(12, 6)")]
        public decimal Longitude { get; set; }
    }
}

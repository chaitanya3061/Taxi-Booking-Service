using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Dal.Entities
{
    public class RideStatus
    {
        [Key] 
        public int Id { get; set; }


        [Column(TypeName = "varchar(20)")]
        public string Status { get; set; }

    }
}

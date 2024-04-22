using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using /*TaxiBookingService.Common.Enums;
using System.ComponentModel.DataAnnotations.Schema;
namespace TaxiBookingService.Dal.Entities
{
    public class DriverRating
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ride")]
        public int RideId { get; set; }

        [Column(TypeName = "varchar(50)")]
       public string Feedback { get; set; }

       public float Rating { get; set; }

       public virtual Ride Ride { get; set; }
    }

}

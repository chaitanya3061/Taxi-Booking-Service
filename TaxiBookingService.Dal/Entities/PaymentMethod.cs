using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TaxiBookingService.Dal.Entities
{
    public class PaymentMethod
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "varchar(30)")]
        public string Name { get; set; }
    }
}

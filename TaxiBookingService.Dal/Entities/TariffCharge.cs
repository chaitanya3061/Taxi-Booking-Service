using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;


namespace TaxiBookingService.Dal.Entities
{
    public class TariffCharge
    {

        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(max)")]
        public string Name { get; set; }

        
        [Column(TypeName = "decimal")]
        public decimal Value { get; set; }
    }
}

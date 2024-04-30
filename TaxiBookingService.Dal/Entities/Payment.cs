using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;


namespace TaxiBookingService.Dal.Entities
{
    public class Payment
    {
        [Key]
        public int Id { get; set; }

        [ForeignKey("Ride")]
        public int RideId {  get; set; }

        [ForeignKey("PaymentMethod")]
        public int PaymentMethodId { get; set; }

        [ForeignKey("PaymentStatus")]
        public int PaymentStatusId { get; set; }

        [Column(TypeName = "datetime2")]
        public DateTime PaymentDate { get; set; }

        public decimal TotalFareAmount { get; set; }

        public virtual Ride Ride { get; set; }
        public virtual PaymentMethod PaymentMethod { get; set; }
        public virtual PaymentStatus PaymentStatus { get; set;}
    }
}

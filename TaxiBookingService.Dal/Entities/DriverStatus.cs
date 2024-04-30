using System.ComponentModel.DataAnnotations;


namespace TaxiBookingService.Dal.Entities
{
    public class DriverStatus
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; }
    }
}

//using TaxiBookingService.Common.Enums;

using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Mvc.Models
{
    public class DriverDetail
    {
        public int id { get; set; }
        public int userId { get; set; }
        public string licenseNumber { get; set; }
        public string driverLocation { get; set; }
        public double? rating { get; set; }
        public double driverBalance { get; set; }
        public DriverStatus status { get; set; }


    }
}

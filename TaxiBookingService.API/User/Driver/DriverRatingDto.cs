using System.ComponentModel.DataAnnotations;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Driver
{
    public class DriverRatingDto
    {
        [PositiveIntegerValidation]
        public int RideId { get; set; }

        public string Feedback { get; set; }

        [Range(1, 5, ErrorMessage = AppConstant.Rating)]
        public float Rating { get; set; }
    }
}

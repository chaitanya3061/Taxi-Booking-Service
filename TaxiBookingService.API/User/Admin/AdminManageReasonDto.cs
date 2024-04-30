using System.ComponentModel.DataAnnotations;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Admin
{
    public class AdminManageReasonDto
    {
      
        [RequiredValidation]
        public string Name { get; set; }

        [Required]
        public bool IsValid { get; set; }

    }
}

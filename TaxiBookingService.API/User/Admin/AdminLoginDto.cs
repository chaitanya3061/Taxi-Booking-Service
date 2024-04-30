using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Admin
{
    public class AdminLoginDto
    {
        [EmailValidation]
        public string Email { get; set; }

        [RequiredValidation]
        public string Password { get; set; }
    }
}

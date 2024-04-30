
namespace TaxiBookingService.API.User.Driver
{
    public class UserLoginDto
    {
        //[EmailValidation]
        public string Email { get; set; }

        //[RequiredValidation]
        public string Password { get; set; }
    }
}


using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
     public class CustomerLoginDto
    {
        [EmailValidation]
        public string Email { get; set; }
        

        [RequiredValidation]
        public string Password { get; set; }
    }
}

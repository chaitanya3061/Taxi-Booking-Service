using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

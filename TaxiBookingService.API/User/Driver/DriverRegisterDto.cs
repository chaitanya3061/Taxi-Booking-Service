using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Driver
{
    public class DriverRegisterDto
    {
        [RequiredValidation]
        public string Name { get; set; }

        //[EmailValidation]
        public string Email { get; set; }


        //[RequiredValidation]
        public string Password { get; set; }

        //[RequiredValidation]
        public string CountryCode { get; set; }


        //[PhoneNumberValidation]
        public string PhoneNumber { get; set; }


        //[LicenceNumberValidation]
        public string LicenseNumber { get; set; }
    }
}

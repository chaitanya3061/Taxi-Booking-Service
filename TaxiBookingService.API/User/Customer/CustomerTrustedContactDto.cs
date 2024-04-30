using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerTrustedContactDto
    {
        
        [RequiredValidation]
        public string ContactName { get; set; }

        [RequiredValidation]
        public string CountryCode { get; set; }

        [PhoneNumberValidation]
        public string ContactPhoneNumber { get; set; }

    }
}

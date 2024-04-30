using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerChangePaymentTypeDTO
    {
        [PositiveIntegerValidation]
        public int CustomerId { get; set; }


        [RequiredValidation]
        public string PaymentType { get; set; }
    }
}

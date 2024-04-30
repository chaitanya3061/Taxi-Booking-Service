using System.ComponentModel.DataAnnotations;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Customer
{
    public class CustomerTopUpDto
    {
        [PositiveIntegerValidation]
        public int CustomerId { get; set; }

        [Required]
        public decimal Amount { get; set; }

        [RequiredValidation]
        public string PaymentMethod { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

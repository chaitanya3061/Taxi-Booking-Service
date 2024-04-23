using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

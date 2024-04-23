using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Common.Attributes;

namespace TaxiBookingService.API.User.Admin
{
    public class AdminManageReasonDto
    {
        [RequiredValidation]
        public string Reason { get; set; }

        [Required]
        public bool IsValid { get; set; }

    }
}

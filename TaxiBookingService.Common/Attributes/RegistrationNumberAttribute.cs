using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using TaxiBookingService.Common.AssetManagement.Common;

namespace TaxiBookingService.Common.Attributes
{
    public class RegistrationNumberAttribute :ValidationAttribute
    {
        private readonly string _errorMessage;
        public RegistrationNumberAttribute(string errorMessage = AppConstant.InvalidRegistration)
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required.");
            }

            string licenseNumber = value.ToString();

            if (!Regex.IsMatch(licenseNumber, @"^[A-Z]{2}\s[0-9]{2}\s[A-Z]{2}\s[0-9]{4}$"));
            {
                return new ValidationResult(_errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}

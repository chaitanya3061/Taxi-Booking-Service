using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace TaxiBookingService.Common.Attributes
{
    public class PhoneNumberValidationAttribute : ValidationAttribute
    {
        private readonly string _errorMessage;

        public PhoneNumberValidationAttribute(string errorMessage = "Invalid phone number format.")
        {
            _errorMessage = errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                ErrorMessage = "Phone number is required.";
                return new ValidationResult($"{validationContext.DisplayName} is required.");
            }

            string phoneNumber = value.ToString();

            if (!Regex.IsMatch(phoneNumber, @"^\d{10}$"))
            {
                return new ValidationResult(_errorMessage);
            }
            return ValidationResult.Success;
        }
    }
}

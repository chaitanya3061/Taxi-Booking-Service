using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Common.Attributes
{
    public class RequiredValidationAttribute : ValidationAttribute
    {
        private readonly int _minLength;
        private readonly int _maxLength;

        public RequiredValidationAttribute(int minLength = 3, int maxLength = 30)
        {
            _minLength = minLength;
            _maxLength = maxLength;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required.");
            }

            string name = value.ToString();

            if (name.Length < _minLength || name.Length > _maxLength)
            {
                return new ValidationResult($"{validationContext.DisplayName} must be between {_minLength} and {_maxLength} characters.");
            }

            return ValidationResult.Success;
        }
    }
}

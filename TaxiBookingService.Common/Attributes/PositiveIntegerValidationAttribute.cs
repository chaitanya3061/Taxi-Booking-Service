using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Common.Attributes
{
    public class PositiveIntegerValidationAttribute : ValidationAttribute
    {
        private readonly int _minValue;
        private readonly int _maxValue;

        public PositiveIntegerValidationAttribute(int minValue = 1, int maxValue = int.MaxValue)
        {
            _minValue = minValue;
            _maxValue = maxValue;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null)
            {
                return new ValidationResult($"{validationContext.DisplayName} is required.");
            }

            if (int.TryParse(value.ToString(), out int intValue))
            {
                if (intValue < _minValue || intValue > _maxValue)
                {
                    return new ValidationResult($"{validationContext.DisplayName} must be between {_minValue} and {_maxValue}.");
                }
                return ValidationResult.Success;
            }

            return new ValidationResult($"{validationContext.DisplayName} must be a valid integer.");
        }
    }
}

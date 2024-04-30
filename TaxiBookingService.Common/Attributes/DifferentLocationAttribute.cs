using System.ComponentModel.DataAnnotations;

namespace TaxiBookingService.Common.Attributes
{
    public class DifferentLocationAttribute : ValidationAttribute
    {
        private readonly string _otherProperty;

        public DifferentLocationAttribute(string otherProperty)
        {
            _otherProperty = otherProperty;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var otherPropertyInfo = validationContext.ObjectType.GetProperty(_otherProperty);
            if (otherPropertyInfo == null)
            {
                return new ValidationResult($"Property {_otherProperty} not found.");
            }

            var otherPropertyValue = otherPropertyInfo.GetValue(validationContext.ObjectInstance, null);

            if (value != null && otherPropertyValue != null && value.Equals(otherPropertyValue))
            {
                return new ValidationResult(ErrorMessage);
            }

            return ValidationResult.Success;
        }
    }
}

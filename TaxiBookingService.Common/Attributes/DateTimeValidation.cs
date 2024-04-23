
using System.ComponentModel.DataAnnotations;
using TaxiBookingService.Common.AssetManagement.Common;

namespace TaxiBookingService.Common.Attributes
{
    public class DateTimeValidation : ValidationAttribute
    {
        private readonly string _errorMessage;
        public DateTimeValidation(int minLength = 3, int maxLength = 30, string errorMessage = AppConstant.InvalidDatetime)
        {
            _errorMessage = errorMessage;
        }
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required.");
            }

            string datetime = value.ToString();

            if (!DateTime.TryParseExact(datetime, "yyyy-MM-dd HH:mm:ss", null, System.Globalization.DateTimeStyles.None, out DateTime parsedDateTime))
            {
                return new ValidationResult(_errorMessage);
            }

            if (parsedDateTime <= DateTime.Now)
            {
                return new ValidationResult(AppConstant.InvalidDatetime);
            }

            return ValidationResult.Success;
        }
    }

}

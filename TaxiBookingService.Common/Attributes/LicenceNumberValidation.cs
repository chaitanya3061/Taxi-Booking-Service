using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using TaxiBookingService.Common.AssetManagement.Common;

namespace TaxiBookingService.Common.Attributes
{
    
    public class LicenceNumberValidation :ValidationAttribute
    {
        private readonly string _errorMessage;
        public LicenceNumberValidation(string errorMessage=AppConstant.InvalidLicence)
        {
                    _errorMessage=errorMessage;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required.");
            }

            string licenseNumber = value.ToString();

            if (!Regex.IsMatch(licenseNumber, @"^[a-zA-Z0-9]{16}$"))
            {
                return new ValidationResult(_errorMessage);
            }

            return ValidationResult.Success;
        }
    }
}


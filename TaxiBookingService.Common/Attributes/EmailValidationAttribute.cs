using AutoMapper.Configuration;
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
    public class EmailValidationAttribute : ValidationAttribute
    {
        private readonly string _errorMessage;
        private readonly int _minLength;
        private readonly int _maxLength;
        public EmailValidationAttribute(int minLength = 3, int maxLength = 30,string errorMessage = AppConstant.InvalidEmail)
        {
            _maxLength = maxLength;
            _minLength = minLength;
            _errorMessage = errorMessage;
        }
        protected override ValidationResult IsValid(object value, System.ComponentModel.DataAnnotations.ValidationContext validationContext)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return new ValidationResult($"{validationContext.DisplayName} is required."); ;
            }

            string Email = value.ToString();

            if (Email.Length < _minLength || Email.Length > _maxLength)
            {
                return new ValidationResult($"Email must be between {_minLength} and {_maxLength} characters.");
            }
            if (!Regex.IsMatch(value.ToString(), @"^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$"))
            {
                return new ValidationResult(_errorMessage);
            }
            return ValidationResult.Success;
        }
    }

}

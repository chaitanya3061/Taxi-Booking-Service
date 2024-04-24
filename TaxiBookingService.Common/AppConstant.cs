using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace TaxiBookingService.Common
{
    namespace AssetManagement.Common
    {
        public class AppConstant
        {
            public const string CannotCancel = "cannot  cancel ride ";
            public const string RetrievedAssetSuccess = "Asset retrived successfully";
            public const string RequestSended = "Request sended successfull";
            public const string IssuedSuccess = "Asset issued successfully";
            public const string RideNotFound = "Ride not found";
            public const string NodriversFound = "No drivers found";
            public const string AssetAlreadyAssigned = "Asset already assigned";
            public const string Error = "An error occurred";
            public const string Delete = "deleted sucessfully";
            public const string Update = "updated successfully";
            public const string Feedback = "Feedback recieved successfully";
            public const string CheckAssignment = "Not Assigned to anyone";
            public const string Notrides = "No rides found";
            public const string MismatchLocation = "locations mismatch";
            public const string InvalidDatetime = "Invalid DateTime";
            public const string InvalidRegistration = " Regsitration number invalid";
            public const string InvalidPhonenumber = "Invalid phone number format";
            public const string InvalidLocation = "Invalid Location";
            public const string UserNotFound = "User not found";
            public const string PasswordNotFound = "password not correct";
            public const string InvalidToken = "Invalid refresh token";
            public const string TokenExpired = "Token expired";
            public const string RegistrationSuccess = "Registration successful";
            public const string LoginSuccess = "Login successful";
            public const string InvalidLicence = "Licence number invalid";
            public const string RequiredEmail = "Email is required ";
            public const string InvalidEmail = "Invalid email format.";
            public const string DriverCancelled = "Ride Cancelled by driver ";
            public const string ErrorOccurred = "An error occurred: {ErrorMessage}";
            public const string Declined = "Ride declined";
            public const string RideEnded = "Ride Ended successfully";
            public const string RegistrationFailed = "Registration failed. Please try again.";
            public const string LoginFailed = "Login failed. Please try again.";
            public const int NotFound = 404;
            public const int ServerError = 500;
            public const string LogoutSuccess = "Logout successful";
            public const string TokenRefreshSuccess = "Token refreshed successfully";
            public const string RideStarted = "ride started sucessffully";
        }

    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Common
{
    namespace AssetManagement.Common
    {
        public class AppConstant
        {
            public const string AssetAddedSuccess = "Asset added successfully";
            public const string RetrievedAssetSuccess = "Asset retrived successfully";
            public const string RequestSended = "Request sended successfull";
            public const string IssuedSuccess = "Asset issued successfully";
            public const string AssetNotFound = "Asset not found";
            public const string AssetAlreadyUnassigned = "Asset already unssigned";
            public const string AssetAlreadyAssigned = "Asset already assigned";
            public const string Error = "An error occurred";
            public const string Delete = "deleted sucessfully";
            public const string Update = "updated successfully";
            public const string OutOfStock = "Stock over";
            public const string CheckAssignment = "Not Assigned to anyone";
            public const string RequestExits = "Request already exists";
            public const string RequestQueue = "Requested sucessfully";
            public const string RequestNotFound = "No request found";
            public const string AddAssignment = "Assignment Added successful(Issued sucessfully)";
            public const string ProcessingAssignment = "Assignment successful it is in processing";
            public const string UnassignedSuccessfullly = "Asset unassigned successfully";
            public const string UnassignedSuccessfullyAndAssigned = "Asset unassigned successfully and assiged to another user";
            public const string UserNotFound = "User not found";
            public const string PasswordNotFound = "password not correct";
            public const string InvalidToken = "Invalid refresh token";
            public const string TokenExpired = "Token expired";
            public const string RegistrationSuccess = "Registration successful";
            public const string LoginSuccess = "Login successful";
            public const string TokenRefreshSuccess = "Token refreshed successfully";
            public const string LogoutSuccess = "Logged out successfully";
            public const string RetrievedAllAssetsSuccess = "Retrieved all assets successfully. assets: {Assets}";
            public const string DriverCancelled = "Ride Cancelled by driver ";
            public const string ErrorOccurred = "An error occurred: {ErrorMessage}";
            public const string Declined = "Ride declined";
            public const string RideEnded = "Ride Ended successfully";
            public const string RegistrationFailed = "Registration failed. Please try again.";
            public const string LoginFailed = "Login failed. Please try again.";
            public const int NotFound = 404;
            public const int ServerError = 500;
            public const string RideStarted = "ride started sucessffully";
        }

    }
}

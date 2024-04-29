
namespace TaxiBookingService.Common
{
    namespace AssetManagement.Common
    {
        public class AppConstant
        {
            public const string CannotCancel = "cannot  cancel ride ";
            public const string RideAccepted = " ride accepeted successfully";
            public const string RequestSended = "Request sended successfull";
            public const string ReasonAdded = "Reason Added successfully";
            public const string RideNotFound = "Ride not found";
            public const string NodriversFound = "No drivers found";
            public const string AddedContacts = "added trusted contact details successfully";
            public const string Error = "An error occurred";
            public const string Delete = "deleted sucessfully";
            public const string Update = "updated successfully";
            public const string Feedback = "Feedback recieved successfully";
            public const string AmountAdded = "Amount added sucessfully";
            public const string Notrides = "No rides found";
            public const string MismatchLocation = "locations are same";
            public const string InvalidDatetime = "Invalid DateTime";
            public const string InvalidRegistration = " Regsitration number invalid";
            public const string InvalidPhonenumber = "Invalid phone number format";
            public const string InvalidLocation = "Invalid Location";
            public const string UserNotFound = "User not found";
            public const string PasswordNotCorrect = "password not correct";
            public const string InvalidToken = "Invalid refresh token";
            public const string TokenExpired = "Token expired";
            public const string RegistrationSuccess = "Registration successful";
            public const string LoginSuccess = "Login successful";
            public const string InvalidLicence = "Licence number invalid";
            public const string RequiredEmail = "Email is required ";
            public const string InvalidEmail = "Invalid email format.";
            public const string DriverCancelled = "Ride Cancelled by driver ";
            public const string CustomerCancelled = "Ride Cancelled by customer ";
            public const string ErrorOccurred = "An error occurred: {ErrorMessage}";
            public const string Declined = "Ride declined succesfully";
            public const string RideEnded = "Ride Ended successfully";
            public const string RegistrationFailed = "Registration failed. Please try again.";
            public const string LoginFailed = "Login failed. Please try again.";
            public const int NotFound = 404;
            public const int PaymentRequired = 402;
            public const int ServerError = 500;
            public const string LogoutSuccess = "Logout successful";
            public const string TokenRefreshSuccess = "Token refreshed successfully";
            public const string RideStarted = "ride started sucessffully";
            public const string TaxiAdded = "Taxi added succesfully";
            public const string CustomerNotFound = "customer not found  Please log in again.";
            public const string DriverNotFound = "Driver not found. Please log in again.";
            public const string NoridesFound = "no rides  found";
            public const string DriverNotAssignedToRide = "Driver is not assigned to this ride.";
            public const string Nomatching = "\"No matching yet\"";
            public static string NotAuthorizedToAccessRide = "You are not authorized to access this ride.";
            public const string DriverNotYetStarted = "Driver is not started to this ride.";
            public const string PaymentNotCompleted = "payment not yet completed";
            public const string PaymentSuccess = "payment successfull";
            public const string PaymentRecieved = "payment recieved";
            public const string PaymentMadeInCash = "Payment made in cash to the driver.";
            public const string PaymentMadeInWallet = "Payment will be made via customer's wallet.";
            public const string CancellationFee = "CancellationFee";
            public const string PerKm = "PerKm";
            public const string Basefare = "Basefare";
            public const string DriverCommissionRate = "driverCommissionRate";
            public const string Wallet = "Wallet";
            public const string Cash = "Cash";
            public const int Pending = 1;
            public const int Completed = 2;
            public const int Searching = 1;
            public const int Accepted = 2;
            public const int Started = 3;
            public const int RideCompleted = 4;
            public const int Cancelled = 5;
            public const int TwoWheeler = 1;
            public const int ThreeWheeler = 2;
            public const int FourWheeler = 3;
            public const int Available = 1;
            public const int Unavailable = 2;
            public const int Admin = 1;
            public const string admin = "Admin";
            public const int Customer = 2;
            public const int Driver = 3;
            public const string RideNotStarted = "ride not started yet";
            public const string TaxiTypeNotFound = "taxi type not found";
            public const string RideAlreadyAccepted = "Ride AlreadyAccepted by driver ";
            public const string EmailAlreadyExists = "Email already exists please log in with another";
            public const string refreshToken = "refreshToken";
            public const string ReasonNotFound = "ReasonNotFound";
            public const string InvalidverificationPin = "InvalidverificationPin";
            public const string CannotUpdateDropOff = "CannotUpdateDrop Off location";
            public const string RetrievedRideSuccess = "ride retrived success";
            public const string StopLocationAdded = "stop location added successfully";
            public const string StopLocationDelete = "stop location deleted successfully";
            public const string CannotAddStopLocation = "cannot add stop location";
            public const string CannotDeleteStopLocation = "Cannot Delete StopLocation";
        }
    }
}

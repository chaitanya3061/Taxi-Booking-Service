
namespace TaxiBookingService.Common
{
    namespace AssetManagement.Common
    {
        public class AppConstant
        {
            public const string CannotCancel = "Cannot cancel ride.";
            public const string RideAccepted = " Ride has been accepeted successfully";
            public const string RequestSent = "Request sent successfull";
            public const string ReasonAdded = "Reason Added successfully";
            public const string RideNotFound = "Ride not found";
            public const string NodriversFound = "No drivers found..";
            public const string AddedContacts = "Added trusted contact details successfully";
            public const string Error = "An error occurred";
            public const string Delete = "Deleted sucessfully";
            public const string Update = "Updated successfully";
            public const string Feedback = "Feedback recieved successfully";
            public const string AmountAdded = "Amount added sucessfully";
            public const string Norides = "No rides found";
            public const string SameLocation = "locations are same";
            public const string InvalidDatetime = "Invalid DateTime";
            public const string InvalidRegistration = " Regsitration number invalid";
            public const string InvalidPhonenumber = "Invalid phone number format";
            public const string InvalidLocation = "Invalid Location";
            public const string UserNotFound = "User not found. Please log in again";
            public const string WrongPassword = "Wrong Password";
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
            public const string DriverNotAllocated = "Driver has Not been Allocated Yet ";
            public static string InvalidRideId = "Ride doesn't Exists";
            public const string DriverNotYetStarted = "Driver has not started the ride.";
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
            public const string admin = "Admin";
            public const string RideNotStarted = "ride has not been started ";
            public const string TaxiTypeNotFound = "taxi type not found";
            public const string RideAlreadyAccepted = "Ride Already Accepted by driver ";
            public const string EmailAlreadyExists = "Email already exists please log in with another";
            public const string refreshToken = "refreshToken";
            public const string ReasonNotFound = "Reason Not Found";
            public const string InvalidverificationPin = "Invalid verification Pin";
            public const string CannotUpdateDropOff = "Cannot Update Drop Off location";
            public const string RetrievedRideSuccess = "ride details retrived successfully";
            public const string StopLocationAdded = "stop location added successfully";
            public const string StopLocationDelete = "stop location deleted successfully";
            public const string CannotAddStopLocation = "cannot add stop location";
            public const string CannotDeleteStopLocation = "Cannot Delete StopLocation";
            public const string CustomerRidesRetrivedSuccess = "Customer Rides Retrived Successfully";
            public const string RideNotCompleted = "Ride Not Completed";
            public const string CommisionRateNotFound = "Commission rate not found in tariff charges.";
            public const string RetrievedDriverSuccess = "Retrieved Driver Details Successfully";
            public const string ScheduleDate = "ScheduleDate cannot be null";
            public const string StatusUpdated = "Status Updated Successfully";
            public const string CannotUpdateStatus = "Cannot change availability status while assigned to a ride.";
            public const string CustomerAlreadyInSearchRide = "Customer already has an active ride request. Please wait for a driver to be assigned.";
            public const string Insufficientfunds = "Insufficient funds in wallet.Please top up.";
            public const string InvalidTopUpAmount = "Top-up amount must be positive.";
            public const string IncorrectAddess = "No results found for the provided address.";
            public const string ErrorMessage = "Failed to retrieve data. Status code";
            public const string RideStatusNotSearching = "Ride status must be 'Searching' in order to accept the ride.";
            public const string Rating= "Rating must be between 1 and 5.";
            public const string PaymentNotFound = "Payment not found";

        }
    }
}

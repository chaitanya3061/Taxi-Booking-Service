
namespace TaxiBookingService.Common.Utilities
{
    public interface ILoggerAdapter
    {
        void LogInformation(string message);
        void LogError(string message, Exception ex);
        void LogWarning(string message);
    }
}

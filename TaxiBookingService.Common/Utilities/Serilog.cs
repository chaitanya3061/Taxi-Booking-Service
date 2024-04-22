using Microsoft.Extensions.Configuration;
using Serilog;



namespace TaxiBookingService.Common.Utilities
{
    public class SerilogAdapter : ILoggerAdapter
    {
        private readonly ILogger logger;

        public SerilogAdapter(IConfiguration configuration)
        {

            logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .WriteTo.Console()
            .WriteTo.File("D:/logs/AssetmanagementLogs-.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();
        }

        public void LogInformation(string message)
        {
            logger.Information(message);
        }

        public void LogError(string message, Exception ex)
        {
            logger.Error(ex, message);
        }

        public void LogWarning(string message)
        {
            logger.Warning(message);
        }
    }
}

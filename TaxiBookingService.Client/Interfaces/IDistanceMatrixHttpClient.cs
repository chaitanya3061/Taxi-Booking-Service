using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Client.Interfaces
{
    public interface IDistanceMatrixHttpClient
    {
        Task<string> GetDurationAsync(Location pickup, Location dropoff);
    }
}


using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Client.DistanceMatrix.Interfaces
{
    public interface IDistanceMatrixHttpClient
    {
        Task<string> GetDurationAsync(Location pickup, Location dropoff);
    }
}

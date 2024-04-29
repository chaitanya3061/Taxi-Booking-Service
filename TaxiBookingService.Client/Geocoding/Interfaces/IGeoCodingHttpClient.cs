using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Client.Geocoding.Interfaces
{
    public interface IGeoCodingHttpClient
    {
        Task<Location> GetGeocodingAsync(string address);
        Task<string> GetReverseGeocodingAsync(decimal latitude, decimal longitude);
    }
}

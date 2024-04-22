using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaxiBookingService.Client.Geocoding.Interfaces
{
    public interface IExternalHttpClient
    {
        Task<(decimal latitude, decimal longitude)> GetGeocodingAsync(string apiKey, string address);

    }
}

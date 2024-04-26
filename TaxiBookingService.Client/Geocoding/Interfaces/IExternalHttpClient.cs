
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Client.Geocoding.Interfaces
{
    public interface IExternalHttpClient
    {
        Task<Location> GetGeocodingAsync(string apiKey, string address);
        Task<string> GetReverseGeocodingAsync(string apiKey, decimal latitude, decimal longitude);


    }
}

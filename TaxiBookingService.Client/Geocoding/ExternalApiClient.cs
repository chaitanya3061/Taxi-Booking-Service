using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Client.Geocoding.Interfaces;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Client.Geocoding
{
    public class ExternalApiClient : IExternalHttpClient
    {
        private readonly HttpClient _client;

        public ExternalApiClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://api.geoapify.com/v1/geocode/");
        }

        public async Task<Location> GetGeocodingAsync(string apiKey, string address)
        {
            string requestUri = $"search?text={Uri.EscapeDataString(address)}&format=json&apiKey={apiKey}";

            HttpResponseMessage response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                if (jsonResponse.results.Count == 0)
                {
                    throw new Exception("No results found for the provided address.");
                }

                var location = new Location
                {
                    Latitude = jsonResponse.results[0].lat,
                    Longitude = jsonResponse.results[0].lon
                };
                return location;
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve data. Status code: {response.StatusCode}");
            }
        }

        public async Task<string> GetReverseGeocodingAsync(string apiKey, decimal latitude, decimal longitude)
        {
            string requestUri = $"reverse?lat={latitude}&lon={longitude}&format=json&apiKey={apiKey}";

            HttpResponseMessage response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                if (jsonResponse.results.Count == 0)
                {
                    throw new Exception("No results found for the provided address.");
                }
                string  address = jsonResponse.results[0].address_line2;
                return address;
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve data. Status code: {response.StatusCode}");
            }
        }


    }
}

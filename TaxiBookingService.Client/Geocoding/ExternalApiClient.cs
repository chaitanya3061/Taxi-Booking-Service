using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TaxiBookingService.Client.Geocoding.Interfaces;

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

        public async Task<(decimal latitude, decimal longitude)> GetGeocodingAsync(string apiKey, string address)
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

                // Extract latitude and longitude from the first result
                decimal latitude = jsonResponse.results[0].lat;
                decimal longitude = jsonResponse.results[0].lon;

                return (latitude, longitude);
            }
            else
            {
                throw new HttpRequestException($"Failed to retrieve data. Status code: {response.StatusCode}");
            }
        }

    }
}

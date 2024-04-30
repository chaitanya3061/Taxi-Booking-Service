using TaxiBookingService.Client.Interfaces;
using TaxiBookingService.Common.AssetManagement.Common;
using TaxiBookingService.Dal.Entities;
using static TaxiBookingService.Common.CustomException;

namespace TaxiBookingService.Client.Geocoding
{
    public class GeoCodingHttpClient : IGeoCodingHttpClient
    {
        private readonly HttpClient _client;
       private readonly string _apiKey; 
        public GeoCodingHttpClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://api.geoapify.com/v1/geocode/");
            _apiKey = "4a87e7d383bb4ca7a8c484db00f43434";
        }

        public async Task<Location> GetGeocodingAsync(string address)
        {
            string requestUri = $"search?text={Uri.EscapeDataString(address)}&format=json&apiKey={_apiKey}";

            HttpResponseMessage response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                if (jsonResponse.results.Count == 0)
                {
                    throw new Exception(AppConstant.IncorrectAddess);
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
                throw new HttpRequestException($"{AppConstant.ErrorMessage} {response.StatusCode}");
            }
        }

        public async Task<string> GetReverseGeocodingAsync( decimal latitude, decimal longitude)
        {
            string requestUri = $"reverse?lat={latitude}&lon={longitude}&format=json&apiKey={_apiKey}";

            HttpResponseMessage response = await _client.GetAsync(requestUri);

            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();

                dynamic jsonResponse = Newtonsoft.Json.JsonConvert.DeserializeObject(responseBody);

                if (jsonResponse.results.Count == 0)
                {
                    throw new Exception(AppConstant.IncorrectAddess);
                }
                string  address = jsonResponse.results[0].formatted;
                return address;
            }
            else
            {
                throw new HttpRequestException($"{AppConstant.ErrorMessage} {response.StatusCode}");
            }
        }


    }
}

using Newtonsoft.Json.Linq;
using TaxiBookingService.Client.DistanceMatrix.Interfaces;
using TaxiBookingService.Dal.Entities;

namespace TaxiBookingService.Client.DistanceMatrix
{
    public class DistanceMatrixHttpClient : IDistanceMatrixHttpClient
    {
        private readonly HttpClient _client;
        private readonly string _apiKey;
        public DistanceMatrixHttpClient(HttpClient client)
        {
            _client = client;
            _client.BaseAddress = new Uri("https://api.distancematrix.ai/maps/api/distancematrix/");
            _apiKey = "PeXBPBEtmt23qZ9fphSE4sZMuYnM7AbI3BvTrA1xC76DC582WB4TNkeK4RekiaoX";
        }

        public async Task<string> GetDurationAsync(Location pickup, Location dropoff)
        {
            string apiUrl = $"json?origins={pickup.Latitude},{pickup.Longitude}&destinations={dropoff.Latitude},{dropoff.Longitude}&key={_apiKey}";
            HttpResponseMessage response = await _client.GetAsync(apiUrl);
            if (response.IsSuccessStatusCode)
            {
                string responseBody = await response.Content.ReadAsStringAsync();
                JObject jsonResponse = JObject.Parse(responseBody);
                string durationText = (string)jsonResponse["rows"][0]["elements"][0]["duration"]["text"];
                return durationText;
            }
            else
            {
                throw new HttpRequestException($"Error: {response.StatusCode}");
            }
        }
    }
}

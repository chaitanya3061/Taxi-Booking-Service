using System.Text.Json.Serialization;

namespace TaxiBookingService.Mvc.Models
{
    public class TokenResponse
    {

        [JsonPropertyName("accessToken")]
        public string AccessToken { get; set; }

        [JsonPropertyName("refreshToken")]
        public string RefreshToken { get; set; }
    }
}

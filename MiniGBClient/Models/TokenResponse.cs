using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace MiniGBClient.Models
{
    public class TokenResponse
    {
        [JsonIgnore]
        [Key]
        public long Id { get; set; }

        [JsonPropertyName("access_token")]
        public string AccessToken { get; set; }

        [JsonPropertyName("token_type")]
        public string TokenType { get; set; }

        [JsonPropertyName("expires_in")]
        public long ExpiresIn { get; set; }

        [JsonPropertyName("scope")]
        public string Scope { get; set; }

        [JsonPropertyName("refresh_token")]
        public string RefreshToken { get; set; }

        [JsonPropertyName("customerResourceURI")]
        public string CustomerResourceURI { get; set; }

        [JsonPropertyName("resourceURI")]
        public string ResourceURI { get; set; }

        [JsonPropertyName("authorizationURI")]
        public string AuthorizationURI { get; set; }
    }
}

using MiniGBClient.Models;
using System.Text;
using System.Text.Json;

namespace MiniGBClient.Services
{
    public class TokenService
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;
        public TokenService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task<TokenResponse?> ExchangeCodeForTokenAsync(string code)
        {
            var tokenURI = _config["GBConnection:TokenURI"];
            var request = new HttpRequestMessage(HttpMethod.Post, tokenURI);
            var basicAuthString = CreateBasicAuthString(_config["client_id"], _config["client_secret"]);
            request.Headers.Add("Authorization", $"Basic {basicAuthString}");
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "authorization_code"),
                new KeyValuePair<string, string>("code", code),
                new KeyValuePair<string, string>("redirect_uri", _config["GBConnection:RedirectURI"])
            });

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            //store json resp
            TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            return tokenResponse;

        }

        public async Task<TokenResponse?> ExchangeRefreshTokenAsync(string refreshToken)
        {
            var tokenURI = _config["GBConnection:TokenURI"];

            var request = new HttpRequestMessage(HttpMethod.Post, tokenURI);
            request.Content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "refresh_token"),
                new KeyValuePair<string, string>("refresh_token", refreshToken)
            });

            var response = await _httpClient.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();
            response.EnsureSuccessStatusCode();
            //store json resp
            TokenResponse tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);
            return tokenResponse;
        }
        public async Task<string> GetRegistrationAccessTokenAsync()
        {
            var tokenURI = _config["GBConnection:TokenURI"];
            var request = new HttpRequestMessage(HttpMethod.Post, tokenURI);
            var basicAuthString = CreateBasicAuthString(_config["client_id"], _config["client_secret"]);
            request.Headers.Add("Authorization", $"Basic {basicAuthString}");
            var content = new FormUrlEncodedContent(new[]
            {
                new KeyValuePair<string, string>("grant_type", "client_credentials"),
                new KeyValuePair<string, string>("scope", "FB=36_40")
            });

            var response = await _httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();

            var responseContent = await response.Content.ReadAsStringAsync();
            //var token = JObject.Parse(responseContent)["access_token"];
            return responseContent;
        }

        private string CreateBasicAuthString(string clientId, string clientSecret)
        {
            if (string.IsNullOrWhiteSpace(clientId) || string.IsNullOrWhiteSpace(clientSecret))
                throw new Exception("Client Id and Client Secret must be provided");
            var authString = $"{clientId}:{clientSecret}";
            var authStringBytes = Encoding.UTF8.GetBytes(authString);
            return Convert.ToBase64String(authStringBytes);
        }
    }
}

using MiniGBClient.Models;

namespace MiniGBClient.Extensions
{
    public static class TokenResponseExtensions
    {
        public static Subscription ToSubscription(this TokenResponse tokenResponse)
        {
            return new Subscription()
            {
                AccessToken = tokenResponse.AccessToken,
                RefreshToken = tokenResponse.RefreshToken,
                CustomerResourceURI = tokenResponse.CustomerResourceURI,
                ResourceURI = tokenResponse.ResourceURI,
                AuthorizationURI = tokenResponse.AuthorizationURI,
                ExpirationTime = System.DateTime.Now.AddSeconds(tokenResponse.ExpiresIn)
            };
        }
    }
}

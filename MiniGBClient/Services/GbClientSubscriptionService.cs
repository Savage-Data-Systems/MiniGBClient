using MiniGBClient.Data;
using MiniGBClient.Models;
using System.Net.Http;

namespace MiniGBClient.Services
{
    /// <summary>
    /// Use this to interact with the GB database and perform CRUD operations.
    /// </summary>
    public class GbClientSubscriptionService
    {
        private readonly MiniGBClientDbContext _context;
        private readonly HttpClient _client;
        private readonly TokenService _tokenService;
        public GbClientSubscriptionService(MiniGBClientDbContext context, HttpClient client, TokenService tokenService)
        {
            _context = context;
            _client = client;
            _tokenService = tokenService;
        }
        public async Task AddSubscriptionAsync(Subscription subscription)
        {
            _context.Subscriptions.Add(subscription);
            await _context.SaveChangesAsync();
        }
        public async Task<Subscription?> GetSubscriptionAsync(long id)
        {
            return await _context.Subscriptions.FindAsync(id);
        }
        public async Task UpdateSubscriptionAsync(Subscription subscription)
        {
            _context.Subscriptions.Update(subscription);
            await _context.SaveChangesAsync();
        }
        public async Task DeleteSubscriptionAsync(Subscription subscription)
        {
            _context.Subscriptions.Remove(subscription);
            await _context.SaveChangesAsync();
        }


        //TODO implement query parameters so we can pass dates to these dc api calls
        protected async Task<string> DownloadResourceBySubscription(Subscription subscription, string resourceURI)
        {
            var request = new HttpRequestMessage(HttpMethod.Get, resourceURI);
            request.Headers.Add("Authorization", $"Bearer {subscription.AccessToken}");

            if (subscription.ExpirationTime < DateTime.Now)
            {
                var tokenResponse = await _tokenService.ExchangeRefreshTokenAsync(subscription.RefreshToken);
                //TODO: Check for failure
                subscription.AccessToken = tokenResponse.AccessToken;
                subscription.ExpirationTime = DateTime.Now.AddSeconds(tokenResponse.ExpiresIn);
                await UpdateSubscriptionAsync(subscription);
            }
            var response = await _client.SendAsync(request);
            var responseContent = await response.Content.ReadAsStringAsync();

            return responseContent;
        }

        public async Task<string> DownloadEUResourceBySubscription(Subscription subscription)
        {
            return await DownloadResourceBySubscription(subscription, subscription.ResourceURI);
        }
        public async Task<string> DownloadCustomerResourcebySubscription(Subscription subscription)
        {
            return await DownloadResourceBySubscription(subscription, subscription.CustomerResourceURI);
        }

       
    }
}

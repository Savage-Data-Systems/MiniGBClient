using System.ComponentModel.DataAnnotations;

namespace MiniGBClient.Models
{
    public class Subscription
    {
        [Key]
        public long Id { get; set; }
        public string AccessToken {get;set;}
        public string RefreshToken { get; set; }
        public string CustomerResourceURI { get; set; }
        public string ResourceURI { get; set; }
        public string AuthorizationURI { get; set; }

        public DateTime ExpirationTime { get; set; }
    }
}
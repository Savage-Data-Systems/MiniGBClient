using System.ComponentModel.DataAnnotations;

namespace MiniGBClient.Models
{
    public class Notification
    {
        [Key]
        public long Id { get; set; }
    }
}
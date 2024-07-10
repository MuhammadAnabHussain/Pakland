using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Pakland.Models
{
    public class BuyingNotifications
    {
        public int Id { get; set; }
        public string Message { get; set; }
        public DateTime Timestamp { get; set; }
        public bool IsSeen { get; set; }
        public string UserId { get; set; }
    }

}


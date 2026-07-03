using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Models
{
    public class ContactMessage
    {
        public int Id { get; set; }
        public string Subject { get; set; }
        public string Message { get; set; }

        public string SendById { get; set; }
        public ApplicationUser Sender { get; set; }
        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public string? Reply { get; set; }
        public string? RepliedById { get; set; }
        public ApplicationUser? RepliedBy { get; set; }
        public DateTime? ReplyedAt { get; set; }

        public bool IsReadByAdmin { get; set; }
        public bool IsReadByCustomer { get; set; }

    }
}

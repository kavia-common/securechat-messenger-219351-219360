using System.ComponentModel.DataAnnotations;

namespace WhatsappBackend.Models
{
    /// <summary>
    /// Stores device push token per user for notifications.
    /// </summary>
    public class DevicePushToken
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; }

        [Required, MaxLength(255)]
        public string Token { get; set; } = string.Empty;

        [MaxLength(64)]
        public string? Platform { get; set; } // ios, android, web

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? User { get; set; }
    }
}

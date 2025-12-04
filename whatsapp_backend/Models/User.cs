using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace WhatsappBackend.Models
{
    /// <summary>
    /// Represents an application user.
    /// </summary>
    public class User
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required, MaxLength(64)]
        public string Username { get; set; } = string.Empty;

        [Required, MaxLength(128)]
        public string DisplayName { get; set; } = string.Empty;

        [MaxLength(256)]
        public string? AvatarUrl { get; set; }

        [MaxLength(32)]
        public string? Phone { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<Contact> Contacts { get; set; } = new List<Contact>();
        public ICollection<ConversationUser> ConversationUsers { get; set; } = new List<ConversationUser>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
        public ICollection<DevicePushToken> DevicePushTokens { get; set; } = new List<DevicePushToken>();
    }
}

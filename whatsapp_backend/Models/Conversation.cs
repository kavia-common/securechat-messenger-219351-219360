using System.ComponentModel.DataAnnotations;

namespace WhatsappBackend.Models
{
    /// <summary>
    /// Conversation represents a chat (group or direct).
    /// </summary>
    public class Conversation
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [MaxLength(128)]
        public string? Title { get; set; }

        public bool IsGroup { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public ICollection<ConversationUser> Participants { get; set; } = new List<ConversationUser>();
        public ICollection<Message> Messages { get; set; } = new List<Message>();
    }

    /// <summary>
    /// Join table for Conversation and User with per-user settings.
    /// </summary>
    public class ConversationUser
    {
        [Required]
        public Guid ConversationId { get; set; }

        [Required]
        public Guid UserId { get; set; }

        public DateTime JoinedAt { get; set; } = DateTime.UtcNow;

        public Conversation? Conversation { get; set; }
        public User? User { get; set; }
    }
}

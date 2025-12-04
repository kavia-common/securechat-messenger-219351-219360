using System.ComponentModel.DataAnnotations;

namespace WhatsappBackend.Models
{
    /// <summary>
    /// A message sent by a user in a conversation.
    /// </summary>
    public class Message
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid ConversationId { get; set; }

        [Required]
        public Guid SenderUserId { get; set; }

        [MaxLength(4096)]
        public string? Content { get; set; }

        public DateTime SentAt { get; set; } = DateTime.UtcNow;

        public Conversation? Conversation { get; set; }
        public User? SenderUser { get; set; }
        public ICollection<Attachment> Attachments { get; set; } = new List<Attachment>();
    }

    /// <summary>
    /// Attachment for a message (image, file, etc.).
    /// </summary>
    public class Attachment
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid MessageId { get; set; }

        [Required, MaxLength(256)]
        public string Url { get; set; } = string.Empty;

        [MaxLength(64)]
        public string? MimeType { get; set; }

        public long? SizeBytes { get; set; }

        public Message? Message { get; set; }
    }
}

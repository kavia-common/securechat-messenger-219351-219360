using System.ComponentModel.DataAnnotations;

namespace WhatsappBackend.Models
{
    /// <summary>
    /// A contact belonging to a user pointing to another user.
    /// </summary>
    public class Contact
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid OwnerUserId { get; set; }

        [Required]
        public Guid ContactUserId { get; set; }

        [MaxLength(128)]
        public string? Alias { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public User? OwnerUser { get; set; }
        public User? ContactUser { get; set; }
    }
}

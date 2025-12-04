using Microsoft.EntityFrameworkCore;
using WhatsappBackend.Models;

namespace WhatsappBackend.Data
{
    /// <summary>
    /// EF Core database context for the WhatsApp-like backend.
    /// </summary>
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<Contact> Contacts => Set<Contact>();
        public DbSet<Conversation> Conversations => Set<Conversation>();
        public DbSet<ConversationUser> ConversationUsers => Set<ConversationUser>();
        public DbSet<Message> Messages => Set<Message>();
        public DbSet<Attachment> Attachments => Set<Attachment>();
        public DbSet<DevicePushToken> DevicePushTokens => Set<DevicePushToken>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // User
            modelBuilder.Entity<User>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Contact
            modelBuilder.Entity<Contact>()
                .HasOne(c => c.OwnerUser)
                .WithMany(u => u.Contacts)
                .HasForeignKey(c => c.OwnerUserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Contact>()
                .HasOne(c => c.ContactUser)
                .WithMany()
                .HasForeignKey(c => c.ContactUserId)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Contact>()
                .HasIndex(c => new { c.OwnerUserId, c.ContactUserId })
                .IsUnique();

            // ConversationUser composite key
            modelBuilder.Entity<ConversationUser>()
                .HasKey(cu => new { cu.ConversationId, cu.UserId });

            modelBuilder.Entity<ConversationUser>()
                .HasOne(cu => cu.Conversation)
                .WithMany(c => c.Participants)
                .HasForeignKey(cu => cu.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<ConversationUser>()
                .HasOne(cu => cu.User)
                .WithMany(u => u.ConversationUsers)
                .HasForeignKey(cu => cu.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            // Message
            modelBuilder.Entity<Message>()
                .HasOne(m => m.Conversation)
                .WithMany(c => c.Messages)
                .HasForeignKey(m => m.ConversationId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Message>()
                .HasOne(m => m.SenderUser)
                .WithMany(u => u.Messages)
                .HasForeignKey(m => m.SenderUserId)
                .OnDelete(DeleteBehavior.Restrict);

            // Attachment
            modelBuilder.Entity<Attachment>()
                .HasOne(a => a.Message)
                .WithMany(m => m.Attachments)
                .HasForeignKey(a => a.MessageId)
                .OnDelete(DeleteBehavior.Cascade);

            // DevicePushToken
            modelBuilder.Entity<DevicePushToken>()
                .HasOne(pt => pt.User)
                .WithMany(u => u.DevicePushTokens)
                .HasForeignKey(pt => pt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<DevicePushToken>()
                .HasIndex(pt => pt.Token)
                .IsUnique();
        }

        /// <summary>
        /// Seed minimal development data when database is empty.
        /// </summary>
        public static async Task EnsureDevSeedAsync(AppDbContext db, CancellationToken ct = default)
        {
            await db.Database.EnsureCreatedAsync(ct);

            if (await db.Users.AnyAsync(ct))
                return;

            var alice = new User { Username = "alice", DisplayName = "Alice" };
            var bob = new User { Username = "bob", DisplayName = "Bob" };
            var charlie = new User { Username = "charlie", DisplayName = "Charlie" };
            db.Users.AddRange(alice, bob, charlie);

            // Contacts
            db.Contacts.AddRange(
                new Contact { OwnerUserId = alice.Id, ContactUserId = bob.Id, Alias = "Bobby" },
                new Contact { OwnerUserId = alice.Id, ContactUserId = charlie.Id },
                new Contact { OwnerUserId = bob.Id, ContactUserId = alice.Id }
            );

            // Direct conversation between Alice and Bob
            var convo = new Conversation { IsGroup = false, Title = null };
            db.Conversations.Add(convo);
            db.ConversationUsers.AddRange(
                new ConversationUser { ConversationId = convo.Id, UserId = alice.Id },
                new ConversationUser { ConversationId = convo.Id, UserId = bob.Id }
            );

            var m1 = new Message
            {
                ConversationId = convo.Id,
                SenderUserId = alice.Id,
                Content = "Hello Bob! 👋",
                SentAt = DateTime.UtcNow.AddMinutes(-10)
            };
            var m2 = new Message
            {
                ConversationId = convo.Id,
                SenderUserId = bob.Id,
                Content = "Hey Alice, good to hear from you!",
                SentAt = DateTime.UtcNow.AddMinutes(-9)
            };
            db.Messages.AddRange(m1, m2);

            await db.SaveChangesAsync(ct);
        }
    }
}

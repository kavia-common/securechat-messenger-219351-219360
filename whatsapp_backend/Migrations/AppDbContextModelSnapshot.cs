#nullable enable
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using WhatsappBackend.Data;

namespace WhatsappBackend.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder.HasAnnotation("ProductVersion", "8.0.10");

            modelBuilder.Entity("WhatsappBackend.Models.Attachment", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string>("MimeType").HasMaxLength(64);
                b.Property<Guid>("MessageId");
                b.Property<long?>("SizeBytes");
                b.Property<string>("Url").IsRequired().HasMaxLength(256);
                b.HasKey("Id");
                b.HasIndex("MessageId");
                b.ToTable("Attachments");
            });

            modelBuilder.Entity("WhatsappBackend.Models.Contact", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string?>("Alias").HasMaxLength(128);
                b.Property<Guid>("ContactUserId");
                b.Property<DateTime>("CreatedAt");
                b.Property<Guid>("OwnerUserId");
                b.HasKey("Id");
                b.HasIndex("ContactUserId");
                b.HasIndex("OwnerUserId", "ContactUserId").IsUnique();
                b.ToTable("Contacts");
            });

            modelBuilder.Entity("WhatsappBackend.Models.Conversation", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<DateTime>("CreatedAt");
                b.Property<bool>("IsGroup");
                b.Property<string?>("Title").HasMaxLength(128);
                b.HasKey("Id");
                b.ToTable("Conversations");
            });

            modelBuilder.Entity("WhatsappBackend.Models.ConversationUser", b =>
            {
                b.Property<Guid>("ConversationId");
                b.Property<Guid>("UserId");
                b.Property<DateTime>("JoinedAt");
                b.HasKey("ConversationId", "UserId");
                b.HasIndex("UserId");
                b.ToTable("ConversationUsers");
            });

            modelBuilder.Entity("WhatsappBackend.Models.DevicePushToken", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<DateTime>("CreatedAt");
                b.Property<string?>("Platform").HasMaxLength(64);
                b.Property<string>("Token").IsRequired().HasMaxLength(255);
                b.Property<Guid>("UserId");
                b.HasKey("Id");
                b.HasIndex("UserId");
                b.HasIndex("Token").IsUnique();
                b.ToTable("DevicePushTokens");
            });

            modelBuilder.Entity("WhatsappBackend.Models.Message", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string?>("Content").HasMaxLength(4096);
                b.Property<Guid>("ConversationId");
                b.Property<DateTime>("SentAt");
                b.Property<Guid>("SenderUserId");
                b.HasKey("Id");
                b.HasIndex("ConversationId");
                b.HasIndex("SenderUserId");
                b.ToTable("Messages");
            });

            modelBuilder.Entity("WhatsappBackend.Models.User", b =>
            {
                b.Property<Guid>("Id").ValueGeneratedOnAdd();
                b.Property<string?>("AvatarUrl").HasMaxLength(256);
                b.Property<DateTime>("CreatedAt");
                b.Property<string>("DisplayName").IsRequired().HasMaxLength(128);
                b.Property<string?>("Phone").HasMaxLength(32);
                b.Property<string>("Username").IsRequired().HasMaxLength(64);
                b.HasKey("Id");
                b.HasIndex("Username").IsUnique();
                b.ToTable("Users");
            });

            modelBuilder.Entity("WhatsappBackend.Models.Attachment", b =>
            {
                b.HasOne("WhatsappBackend.Models.Message", "Message")
                 .WithMany("Attachments")
                 .HasForeignKey("MessageId")
                 .OnDelete(DeleteBehavior.Cascade)
                 .IsRequired();
            });

            modelBuilder.Entity("WhatsappBackend.Models.Contact", b =>
            {
                b.HasOne("WhatsappBackend.Models.User", "OwnerUser")
                 .WithMany("Contacts")
                 .HasForeignKey("OwnerUserId")
                 .OnDelete(DeleteBehavior.Cascade)
                 .IsRequired();

                b.HasOne("WhatsappBackend.Models.User", "ContactUser")
                 .WithMany()
                 .HasForeignKey("ContactUserId")
                 .OnDelete(DeleteBehavior.Restrict)
                 .IsRequired();
            });

            modelBuilder.Entity("WhatsappBackend.Models.ConversationUser", b =>
            {
                b.HasOne("WhatsappBackend.Models.Conversation", "Conversation")
                 .WithMany("Participants")
                 .HasForeignKey("ConversationId")
                 .OnDelete(DeleteBehavior.Cascade)
                 .IsRequired();

                b.HasOne("WhatsappBackend.Models.User", "User")
                 .WithMany("ConversationUsers")
                 .HasForeignKey("UserId")
                 .OnDelete(DeleteBehavior.Cascade)
                 .IsRequired();
            });

            modelBuilder.Entity("WhatsappBackend.Models.DevicePushToken", b =>
            {
                b.HasOne("WhatsappBackend.Models.User", "User")
                 .WithMany("DevicePushTokens")
                 .HasForeignKey("UserId")
                 .OnDelete(DeleteBehavior.Cascade)
                 .IsRequired();
            });

            modelBuilder.Entity("WhatsappBackend.Models.Message", b =>
            {
                b.HasOne("WhatsappBackend.Models.Conversation", "Conversation")
                 .WithMany("Messages")
                 .HasForeignKey("ConversationId")
                 .OnDelete(DeleteBehavior.Cascade)
                 .IsRequired();

                b.HasOne("WhatsappBackend.Models.User", "SenderUser")
                 .WithMany("Messages")
                 .HasForeignKey("SenderUserId")
                 .OnDelete(DeleteBehavior.Restrict)
                 .IsRequired();
            });

            modelBuilder.Entity("WhatsappBackend.Models.Conversation", b => { });
            modelBuilder.Entity("WhatsappBackend.Models.Message", b => { });
            modelBuilder.Entity("WhatsappBackend.Models.User", b => { });
#pragma warning restore 612, 618
        }
    }
}

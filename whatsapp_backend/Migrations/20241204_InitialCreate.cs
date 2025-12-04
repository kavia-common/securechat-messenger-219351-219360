using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatsappBackend.Migrations
{
    /// <summary>
    /// Initial schema for users, contacts, conversations, messages, attachments, and device tokens.
    /// </summary>
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Username = table.Column<string>(maxLength: 64, nullable: false),
                    DisplayName = table.Column<string>(maxLength: 128, nullable: false),
                    AvatarUrl = table.Column<string>(maxLength: 256, nullable: true),
                    Phone = table.Column<string>(maxLength: 32, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Conversations",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Title = table.Column<string>(maxLength: 128, nullable: true),
                    IsGroup = table.Column<bool>(nullable: false),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Conversations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Contacts",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    OwnerUserId = table.Column<Guid>(nullable: false),
                    ContactUserId = table.Column<Guid>(nullable: false),
                    Alias = table.Column<string>(maxLength: 128, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Contacts", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Contacts_Users_ContactUserId",
                        column: x => x.ContactUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Contacts_Users_OwnerUserId",
                        column: x => x.OwnerUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ConversationUsers",
                columns: table => new
                {
                    ConversationId = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    JoinedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ConversationUsers", x => new { x.ConversationId, x.UserId });
                    table.ForeignKey(
                        name: "FK_ConversationUsers_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ConversationUsers_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Messages",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    ConversationId = table.Column<Guid>(nullable: false),
                    SenderUserId = table.Column<Guid>(nullable: false),
                    Content = table.Column<string>(maxLength: 4096, nullable: true),
                    SentAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Messages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Messages_Conversations_ConversationId",
                        column: x => x.ConversationId,
                        principalTable: "Conversations",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Messages_Users_SenderUserId",
                        column: x => x.SenderUserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Attachments",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MessageId = table.Column<Guid>(nullable: false),
                    Url = table.Column<string>(maxLength: 256, nullable: false),
                    MimeType = table.Column<string>(maxLength: 64, nullable: true),
                    SizeBytes = table.Column<long>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Attachments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Attachments_Messages_MessageId",
                        column: x => x.MessageId,
                        principalTable: "Messages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "DevicePushTokens",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    UserId = table.Column<Guid>(nullable: false),
                    Token = table.Column<string>(maxLength: 255, nullable: false),
                    Platform = table.Column<string>(maxLength: 64, nullable: true),
                    CreatedAt = table.Column<DateTime>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DevicePushTokens", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DevicePushTokens_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            // Indexes
            migrationBuilder.CreateIndex(
                name: "IX_Users_Username",
                table: "Users",
                column: "Username",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_ContactUserId",
                table: "Contacts",
                column: "ContactUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_OwnerUserId_ContactUserId",
                table: "Contacts",
                columns: new[] { "OwnerUserId", "ContactUserId" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ConversationUsers_UserId",
                table: "ConversationUsers",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_ConversationId",
                table: "Messages",
                column: "ConversationId");

            migrationBuilder.CreateIndex(
                name: "IX_Messages_SenderUserId",
                table: "Messages",
                column: "SenderUserId");

            migrationBuilder.CreateIndex(
                name: "IX_Attachments_MessageId",
                table: "Attachments",
                column: "MessageId");

            migrationBuilder.CreateIndex(
                name: "IX_DevicePushTokens_UserId",
                table: "DevicePushTokens",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_DevicePushTokens_Token",
                table: "DevicePushTokens",
                column: "Token",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Attachments");
            migrationBuilder.DropTable(name: "Contacts");
            migrationBuilder.DropTable(name: "ConversationUsers");
            migrationBuilder.DropTable(name: "DevicePushTokens");
            migrationBuilder.DropTable(name: "Messages");
            migrationBuilder.DropTable(name: "Conversations");
            migrationBuilder.DropTable(name: "Users");
        }
    }
}

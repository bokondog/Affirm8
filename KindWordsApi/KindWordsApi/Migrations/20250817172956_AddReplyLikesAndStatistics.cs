using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace KindWordsApi.Migrations
{
    /// <inheritdoc />
    public partial class AddReplyLikesAndStatistics : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsLikedByMessageOwner",
                table: "Replies",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "LikeCount",
                table: "Replies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "ReplyLikes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReplyId = table.Column<int>(type: "int", nullable: false),
                    MessageOwnerId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LikedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReplyLikes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReplyLikes_Replies_ReplyId",
                        column: x => x.ReplyId,
                        principalTable: "Replies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReplyLikes_Users_MessageOwnerId",
                        column: x => x.MessageOwnerId,
                        principalTable: "Users",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReplyLikes_MessageOwnerId",
                table: "ReplyLikes",
                column: "MessageOwnerId");

            migrationBuilder.CreateIndex(
                name: "IX_ReplyLikes_ReplyId_MessageOwnerId",
                table: "ReplyLikes",
                columns: new[] { "ReplyId", "MessageOwnerId" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReplyLikes");

            migrationBuilder.DropColumn(
                name: "IsLikedByMessageOwner",
                table: "Replies");

            migrationBuilder.DropColumn(
                name: "LikeCount",
                table: "Replies");
        }
    }
}

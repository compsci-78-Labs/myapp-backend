using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddRefreshTokens : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "refresh_token_expires_at",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_hash_token",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_id",
                table: "users");

            migrationBuilder.DropColumn(
                name: "refresh_token_revoked_at",
                table: "users");

            migrationBuilder.CreateTable(
                name: "refresh_tokens",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    hash_token = table.Column<string>(type: "character varying(255)", maxLength: 255, nullable: false),
                    user_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    expires_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    revoked_at = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    user_id1 = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_refresh_tokens", x => x.id);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id",
                        column: x => x.user_id,
                        principalTable: "users",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_refresh_tokens_users_user_id1",
                        column: x => x.user_id1,
                        principalTable: "users",
                        principalColumn: "id");
                });

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_hash_token",
                table: "refresh_tokens",
                column: "hash_token",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id",
                table: "refresh_tokens",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id1",
                table: "refresh_tokens",
                column: "user_id1",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "refresh_tokens");

            migrationBuilder.AddColumn<DateTime>(
                name: "refresh_token_expires_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "refresh_token_hash_token",
                table: "users",
                type: "character varying(255)",
                maxLength: 255,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "refresh_token_id",
                table: "users",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "refresh_token_revoked_at",
                table: "users",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}

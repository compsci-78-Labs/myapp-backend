using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemoveDuplicateRefreshTokenRelationship : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_refresh_tokens_users_user_id1",
                table: "refresh_tokens");

            migrationBuilder.DropIndex(
                name: "ix_refresh_tokens_user_id1",
                table: "refresh_tokens");

            migrationBuilder.DropColumn(
                name: "user_id1",
                table: "refresh_tokens");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "user_id1",
                table: "refresh_tokens",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_refresh_tokens_user_id1",
                table: "refresh_tokens",
                column: "user_id1",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_refresh_tokens_users_user_id1",
                table: "refresh_tokens",
                column: "user_id1",
                principalTable: "users",
                principalColumn: "id");
        }
    }
}

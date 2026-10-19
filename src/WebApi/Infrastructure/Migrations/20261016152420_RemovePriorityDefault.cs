using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WebApi.Data.Migrations
{
    /// <inheritdoc />
    public partial class RemovePriorityDefault : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "priority",
                table: "task_items",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldDefaultValue: "Medium");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "priority",
                table: "task_items",
                type: "character varying(20)",
                maxLength: 20,
                nullable: false,
                defaultValue: "Medium",
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20);
        }
    }
}

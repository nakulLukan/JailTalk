using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AppUserIsSystemGeneratedUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_system_generated_user",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_system_generated_user",
                table: "AspNetUsers");
        }
    }
}

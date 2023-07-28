using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UserFullNameAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "full_name",
                table: "AspNetUsers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "full_name",
                table: "AspNetUsers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AppUserJailIdAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "prison_id",
                table: "AspNetUsers",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_prison_id",
                table: "AspNetUsers",
                column: "prison_id");

            migrationBuilder.AddForeignKey(
                name: "fk_asp_net_users_jails_prison_id",
                table: "AspNetUsers",
                column: "prison_id",
                principalTable: "jails",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_asp_net_users_jails_prison_id",
                table: "AspNetUsers");

            migrationBuilder.DropIndex(
                name: "ix_asp_net_users_prison_id",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "prison_id",
                table: "AspNetUsers");
        }
    }
}

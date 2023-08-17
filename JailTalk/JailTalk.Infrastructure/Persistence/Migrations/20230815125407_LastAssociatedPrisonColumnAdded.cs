using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class LastAssociatedPrisonColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "last_associated_jail_id",
                table: "prisoner_functions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_functions_last_associated_jail_id",
                table: "prisoner_functions",
                column: "last_associated_jail_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_functions_jails_last_associated_jail_id",
                table: "prisoner_functions",
                column: "last_associated_jail_id",
                principalTable: "jails",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_functions_jails_last_associated_jail_id",
                table: "prisoner_functions");

            migrationBuilder.DropIndex(
                name: "ix_prisoner_functions_last_associated_jail_id",
                table: "prisoner_functions");

            migrationBuilder.DropColumn(
                name: "last_associated_jail_id",
                table: "prisoner_functions");
        }
    }
}

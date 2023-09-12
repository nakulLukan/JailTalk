using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DpColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "dp_attachment_id",
                table: "prisoner_functions",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_functions_dp_attachment_id",
                table: "prisoner_functions",
                column: "dp_attachment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_functions_attachments_dp_attachment_id",
                table: "prisoner_functions",
                column: "dp_attachment_id",
                principalTable: "attachments",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_functions_attachments_dp_attachment_id",
                table: "prisoner_functions");

            migrationBuilder.DropIndex(
                name: "ix_prisoner_functions_dp_attachment_id",
                table: "prisoner_functions");

            migrationBuilder.DropColumn(
                name: "dp_attachment_id",
                table: "prisoner_functions");
        }
    }
}

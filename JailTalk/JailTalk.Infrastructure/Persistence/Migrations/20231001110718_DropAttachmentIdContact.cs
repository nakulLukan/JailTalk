using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class DropAttachmentIdContact : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_phone_directory_attachments_id_proof_attachment_id",
                table: "phone_directory");

            migrationBuilder.DropIndex(
                name: "ix_phone_directory_id_proof_attachment_id",
                table: "phone_directory");

            migrationBuilder.DropColumn(
                name: "id_proof_attachment_id",
                table: "phone_directory");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_proof_attachment_id",
                table: "phone_directory",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_phone_directory_id_proof_attachment_id",
                table: "phone_directory",
                column: "id_proof_attachment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_phone_directory_attachments_id_proof_attachment_id",
                table: "phone_directory",
                column: "id_proof_attachment_id",
                principalTable: "attachments",
                principalColumn: "id");
        }
    }
}

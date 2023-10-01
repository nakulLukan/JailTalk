using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactMultipleAttachments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "attachment_phone_directory",
                columns: table => new
                {
                    id_proof_attachments_id = table.Column<int>(type: "integer", nullable: false),
                    phone_directory_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachment_phone_directory", x => new { x.id_proof_attachments_id, x.phone_directory_id });
                    table.ForeignKey(
                        name: "fk_attachment_phone_directory_attachments_id_proof_attachments",
                        column: x => x.id_proof_attachments_id,
                        principalTable: "attachments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_attachment_phone_directory_phone_directory_phone_directory_",
                        column: x => x.phone_directory_id,
                        principalTable: "phone_directory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_attachment_phone_directory_phone_directory_id",
                table: "attachment_phone_directory",
                column: "phone_directory_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "attachment_phone_directory");
        }
    }
}

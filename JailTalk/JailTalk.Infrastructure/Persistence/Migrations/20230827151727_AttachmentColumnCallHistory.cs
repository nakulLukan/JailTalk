using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AttachmentColumnCallHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "call_recording_attachment_id",
                table: "call_history",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "ix_call_history_call_recording_attachment_id",
                table: "call_history",
                column: "call_recording_attachment_id");

            migrationBuilder.AddForeignKey(
                name: "fk_call_history_attachments_call_recording_attachment_id",
                table: "call_history",
                column: "call_recording_attachment_id",
                principalTable: "attachments",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_call_history_attachments_call_recording_attachment_id",
                table: "call_history");

            migrationBuilder.DropIndex(
                name: "ix_call_history_call_recording_attachment_id",
                table: "call_history");

            migrationBuilder.DropColumn(
                name: "call_recording_attachment_id",
                table: "call_history");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ContactAttachmentDetails : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "id_proof_attachment_id",
                table: "phone_directory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "id_proof_type_id",
                table: "phone_directory",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "id_proof_value",
                table: "phone_directory",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.InsertData(
                table: "lookup_masters",
                columns: new[] { "id", "internal_name", "is_active", "name" },
                values: new object[] { 4, "IdProof", true, "ID Proof" });

            migrationBuilder.InsertData(
                table: "lookup_details",
                columns: new[] { "id", "internal_name", "is_active", "lookup_master_id", "order", "value" },
                values: new object[,]
                {
                    { 9, "aadhar", true, 4, 1, "Aadhar" },
                    { 10, "driving_license", true, 4, 2, "Driving License" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_phone_directory_id_proof_attachment_id",
                table: "phone_directory",
                column: "id_proof_attachment_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_directory_id_proof_type_id",
                table: "phone_directory",
                column: "id_proof_type_id");

            migrationBuilder.AddForeignKey(
                name: "fk_phone_directory_attachments_id_proof_attachment_id",
                table: "phone_directory",
                column: "id_proof_attachment_id",
                principalTable: "attachments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_phone_directory_lookup_details_id_proof_type_id",
                table: "phone_directory",
                column: "id_proof_type_id",
                principalTable: "lookup_details",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_phone_directory_attachments_id_proof_attachment_id",
                table: "phone_directory");

            migrationBuilder.DropForeignKey(
                name: "fk_phone_directory_lookup_details_id_proof_type_id",
                table: "phone_directory");

            migrationBuilder.DropIndex(
                name: "ix_phone_directory_id_proof_attachment_id",
                table: "phone_directory");

            migrationBuilder.DropIndex(
                name: "ix_phone_directory_id_proof_type_id",
                table: "phone_directory");

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "lookup_masters",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DropColumn(
                name: "id_proof_attachment_id",
                table: "phone_directory");

            migrationBuilder.DropColumn(
                name: "id_proof_type_id",
                table: "phone_directory");

            migrationBuilder.DropColumn(
                name: "id_proof_value",
                table: "phone_directory");
        }
    }
}

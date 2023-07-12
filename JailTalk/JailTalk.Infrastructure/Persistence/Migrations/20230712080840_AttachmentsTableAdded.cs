using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AttachmentsTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_face_encoding_mapping_app_face_encoding_face_encod",
                table: "prisoner_face_encoding_mapping");

            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_face_encoding_mapping_prisoners_prisoner_id",
                table: "prisoner_face_encoding_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prisoner_face_encoding_mapping",
                table: "prisoner_face_encoding_mapping");

            migrationBuilder.DropPrimaryKey(
                name: "pk_app_face_encoding",
                table: "app_face_encoding");

            migrationBuilder.RenameTable(
                name: "prisoner_face_encoding_mapping",
                newName: "prisoner_face_encoding_mappings");

            migrationBuilder.RenameTable(
                name: "app_face_encoding",
                newName: "app_face_encodings");

            migrationBuilder.RenameIndex(
                name: "ix_prisoner_face_encoding_mapping_prisoner_id",
                table: "prisoner_face_encoding_mappings",
                newName: "ix_prisoner_face_encoding_mappings_prisoner_id");

            migrationBuilder.RenameIndex(
                name: "ix_prisoner_face_encoding_mapping_face_encoding_id",
                table: "prisoner_face_encoding_mappings",
                newName: "ix_prisoner_face_encoding_mappings_face_encoding_id");

            migrationBuilder.AddColumn<int>(
                name: "image_id",
                table: "prisoner_face_encoding_mappings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "pk_prisoner_face_encoding_mappings",
                table: "prisoner_face_encoding_mappings",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_app_face_encodings",
                table: "app_face_encodings",
                column: "id");

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data = table.Column<byte[]>(type: "bytea", nullable: true),
                    path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_blob = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    file_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachments", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_face_encoding_mappings_image_id",
                table: "prisoner_face_encoding_mappings",
                column: "image_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_face_encoding_mappings_app_face_encodings_face_enc",
                table: "prisoner_face_encoding_mappings",
                column: "face_encoding_id",
                principalTable: "app_face_encodings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_face_encoding_mappings_attachments_attachment_id",
                table: "prisoner_face_encoding_mappings",
                column: "image_id",
                principalTable: "attachments",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_face_encoding_mappings_prisoners_prisoner_id",
                table: "prisoner_face_encoding_mappings",
                column: "prisoner_id",
                principalTable: "prisoners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_face_encoding_mappings_app_face_encodings_face_enc",
                table: "prisoner_face_encoding_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_face_encoding_mappings_attachments_attachment_id",
                table: "prisoner_face_encoding_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_face_encoding_mappings_prisoners_prisoner_id",
                table: "prisoner_face_encoding_mappings");

            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prisoner_face_encoding_mappings",
                table: "prisoner_face_encoding_mappings");

            migrationBuilder.DropIndex(
                name: "ix_prisoner_face_encoding_mappings_image_id",
                table: "prisoner_face_encoding_mappings");

            migrationBuilder.DropPrimaryKey(
                name: "pk_app_face_encodings",
                table: "app_face_encodings");

            migrationBuilder.DropColumn(
                name: "image_id",
                table: "prisoner_face_encoding_mappings");

            migrationBuilder.RenameTable(
                name: "prisoner_face_encoding_mappings",
                newName: "prisoner_face_encoding_mapping");

            migrationBuilder.RenameTable(
                name: "app_face_encodings",
                newName: "app_face_encoding");

            migrationBuilder.RenameIndex(
                name: "ix_prisoner_face_encoding_mappings_prisoner_id",
                table: "prisoner_face_encoding_mapping",
                newName: "ix_prisoner_face_encoding_mapping_prisoner_id");

            migrationBuilder.RenameIndex(
                name: "ix_prisoner_face_encoding_mappings_face_encoding_id",
                table: "prisoner_face_encoding_mapping",
                newName: "ix_prisoner_face_encoding_mapping_face_encoding_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prisoner_face_encoding_mapping",
                table: "prisoner_face_encoding_mapping",
                column: "id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_app_face_encoding",
                table: "app_face_encoding",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_face_encoding_mapping_app_face_encoding_face_encod",
                table: "prisoner_face_encoding_mapping",
                column: "face_encoding_id",
                principalTable: "app_face_encoding",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_face_encoding_mapping_prisoners_prisoner_id",
                table: "prisoner_face_encoding_mapping",
                column: "prisoner_id",
                principalTable: "prisoners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

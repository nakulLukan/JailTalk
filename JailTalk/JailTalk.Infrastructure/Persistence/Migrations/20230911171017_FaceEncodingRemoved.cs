using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FaceEncodingRemoved : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_face_encoding_mappings_app_face_encodings_face_enc",
                table: "prisoner_face_encoding_mappings");

            migrationBuilder.DropForeignKey(
                name: "fk_prisoners_address_book_address_id",
                table: "prisoners");

            migrationBuilder.DropTable(
                name: "app_face_encodings");

            migrationBuilder.DropIndex(
                name: "ix_prisoners_address_id",
                table: "prisoners");

            migrationBuilder.DropIndex(
                name: "ix_prisoner_face_encoding_mappings_face_encoding_id",
                table: "prisoner_face_encoding_mappings");

            migrationBuilder.DropColumn(
                name: "address_id",
                table: "prisoners");

            migrationBuilder.DropColumn(
                name: "face_encoding_id",
                table: "prisoner_face_encoding_mappings");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "address_id",
                table: "prisoners",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "face_encoding_id",
                table: "prisoner_face_encoding_mappings",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "app_face_encodings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    encoding = table.Column<double[]>(type: "double precision[]", nullable: false),
                    encoding_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    last_modified_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_modified_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_face_encodings", x => x.id);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prisoners_address_id",
                table: "prisoners",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_face_encoding_mappings_face_encoding_id",
                table: "prisoner_face_encoding_mappings",
                column: "face_encoding_id",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_face_encoding_mappings_app_face_encodings_face_enc",
                table: "prisoner_face_encoding_mappings",
                column: "face_encoding_id",
                principalTable: "app_face_encodings",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "fk_prisoners_address_book_address_id",
                table: "prisoners",
                column: "address_id",
                principalTable: "address_book",
                principalColumn: "id");
        }
    }
}

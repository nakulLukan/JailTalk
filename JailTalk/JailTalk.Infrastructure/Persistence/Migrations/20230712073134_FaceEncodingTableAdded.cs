using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class FaceEncodingTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "app_face_encoding",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    encoding = table.Column<double[]>(type: "double precision[]", nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    last_modified_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    last_modified_by = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    encoding_name = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_app_face_encoding", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "prisoner_face_encoding_mapping",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    face_encoding_id = table.Column<int>(type: "integer", nullable: false),
                    prisoner_id = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prisoner_face_encoding_mapping", x => x.id);
                    table.ForeignKey(
                        name: "fk_prisoner_face_encoding_mapping_app_face_encoding_face_encod",
                        column: x => x.face_encoding_id,
                        principalTable: "app_face_encoding",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prisoner_face_encoding_mapping_prisoners_prisoner_id",
                        column: x => x.prisoner_id,
                        principalTable: "prisoners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_face_encoding_mapping_face_encoding_id",
                table: "prisoner_face_encoding_mapping",
                column: "face_encoding_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_face_encoding_mapping_prisoner_id",
                table: "prisoner_face_encoding_mapping",
                column: "prisoner_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prisoner_face_encoding_mapping");

            migrationBuilder.DropTable(
                name: "app_face_encoding");
        }
    }
}

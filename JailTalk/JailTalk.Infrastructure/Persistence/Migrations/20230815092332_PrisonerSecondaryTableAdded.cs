using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PrisonerSecondaryTableAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "allow_unlimited_calls_till",
                table: "prisoners");

            migrationBuilder.CreateTable(
                name: "prisoner_function",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prisoner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    last_released_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    punishment_ends_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    unlimited_calls_ends_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prisoner_function", x => x.id);
                    table.ForeignKey(
                        name: "fk_prisoner_function_prisoners_prisoner_id",
                        column: x => x.prisoner_id,
                        principalTable: "prisoners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_function_prisoner_id",
                table: "prisoner_function",
                column: "prisoner_id",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "prisoner_function");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "allow_unlimited_calls_till",
                table: "prisoners",
                type: "timestamp with time zone",
                nullable: true);
        }
    }
}

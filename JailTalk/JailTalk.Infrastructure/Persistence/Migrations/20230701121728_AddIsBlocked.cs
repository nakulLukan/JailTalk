using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddIsBlocked : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_active",
                table: "prisoners",
                type: "boolean",
                nullable: false,
                defaultValue: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_blocked",
                table: "prisoners",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "application_setting",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_setting", x => x.id);
                });

            migrationBuilder.InsertData(
                table: "application_setting",
                columns: new[] { "id", "description", "value" },
                values: new object[,]
                {
                    { 1, "Max number of contacts to show to a prisoner", "3" },
                    { 2, "Max number of minutes a male prisoner can make a call.", "15" },
                    { 3, "Max number of minutes a female prisoner can make a call.", "20" },
                    { 4, "Max number of minutes a LGBTQ+ prisoner can make a call.", "15" }
                });

            migrationBuilder.UpdateData(
                table: "jails",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_on", "updated_on" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_setting");

            migrationBuilder.DropColumn(
                name: "is_active",
                table: "prisoners");

            migrationBuilder.DropColumn(
                name: "is_blocked",
                table: "prisoners");

            migrationBuilder.UpdateData(
                table: "jails",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_on", "updated_on" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 1, 7, 35, 33, 496, DateTimeKind.Unspecified).AddTicks(9712), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 1, 7, 35, 33, 496, DateTimeKind.Unspecified).AddTicks(9721), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}

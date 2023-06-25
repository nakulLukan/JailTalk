using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class SeederAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "pin_code",
                table: "address_book",
                type: "character varying(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.InsertData(
                table: "jails",
                columns: new[] { "id", "address_id", "code", "created_by", "created_on", "name", "updated_by", "updated_on" },
                values: new object[] { 1, null, "MLP-PN-SJ", null, new DateTimeOffset(new DateTime(2023, 6, 25, 12, 12, 30, 624, DateTimeKind.Unspecified).AddTicks(1341), new TimeSpan(0, 0, 0, 0, 0)), "Ponnani Sub Jail", null, new DateTimeOffset(new DateTime(2023, 6, 25, 12, 12, 30, 624, DateTimeKind.Unspecified).AddTicks(1347), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "lookup_masters",
                columns: new[] { "id", "internal_name", "is_active", "name" },
                values: new object[,]
                {
                    { 1, "state", true, "States" },
                    { 2, "country", true, "Countries" }
                });

            migrationBuilder.InsertData(
                table: "lookup_details",
                columns: new[] { "id", "internal_name", "is_active", "lookup_master_id", "order", "value" },
                values: new object[,]
                {
                    { 1, "kerala", true, 1, 1, "Kerala" },
                    { 2, "india", true, 2, 1, "India" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "jails",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "lookup_masters",
                keyColumn: "id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "lookup_masters",
                keyColumn: "id",
                keyValue: 2);

            migrationBuilder.DropColumn(
                name: "pin_code",
                table: "address_book");
        }
    }
}

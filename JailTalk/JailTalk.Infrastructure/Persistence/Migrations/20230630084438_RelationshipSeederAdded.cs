using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipSeederAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "jails",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_on", "updated_on" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 6, 30, 8, 44, 38, 677, DateTimeKind.Unspecified).AddTicks(7640), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 6, 30, 8, 44, 38, 677, DateTimeKind.Unspecified).AddTicks(7647), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "lookup_masters",
                keyColumn: "id",
                keyValue: 1,
                column: "internal_name",
                value: "State");

            migrationBuilder.UpdateData(
                table: "lookup_masters",
                keyColumn: "id",
                keyValue: 2,
                column: "internal_name",
                value: "Country");

            migrationBuilder.InsertData(
                table: "lookup_masters",
                columns: new[] { "id", "internal_name", "is_active", "name" },
                values: new object[] { 3, "Relationship", true, "Relationship" });

            migrationBuilder.InsertData(
                table: "lookup_details",
                columns: new[] { "id", "internal_name", "is_active", "lookup_master_id", "order", "value" },
                values: new object[,]
                {
                    { 3, "father", true, 3, 1, "Father" },
                    { 4, "mother", true, 3, 2, "Mother" },
                    { 5, "brother", true, 3, 3, "Brother" },
                    { 6, "sister", true, 3, 4, "Sister" },
                    { 7, "lawyer", true, 3, 5, "Lawyer" },
                    { 8, "other", true, 3, 6, "Others" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "lookup_details",
                keyColumn: "id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "lookup_masters",
                keyColumn: "id",
                keyValue: 3);

            migrationBuilder.UpdateData(
                table: "jails",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_on", "updated_on" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 6, 25, 12, 12, 30, 624, DateTimeKind.Unspecified).AddTicks(1341), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 6, 25, 12, 12, 30, 624, DateTimeKind.Unspecified).AddTicks(1347), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "lookup_masters",
                keyColumn: "id",
                keyValue: 1,
                column: "internal_name",
                value: "state");

            migrationBuilder.UpdateData(
                table: "lookup_masters",
                keyColumn: "id",
                keyValue: 2,
                column: "internal_name",
                value: "country");
        }
    }
}

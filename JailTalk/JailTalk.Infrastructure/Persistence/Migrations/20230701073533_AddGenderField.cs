using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddGenderField : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "gender",
                table: "prisoners",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            migrationBuilder.UpdateData(
                table: "jails",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_on", "updated_on" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 7, 1, 7, 35, 33, 496, DateTimeKind.Unspecified).AddTicks(9712), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 7, 1, 7, 35, 33, 496, DateTimeKind.Unspecified).AddTicks(9721), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "gender",
                table: "prisoners");

            migrationBuilder.UpdateData(
                table: "jails",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "created_on", "updated_on" },
                values: new object[] { new DateTimeOffset(new DateTime(2023, 6, 30, 8, 44, 38, 677, DateTimeKind.Unspecified).AddTicks(7640), new TimeSpan(0, 0, 0, 0, 0)), new DateTimeOffset(new DateTime(2023, 6, 30, 8, 44, 38, 677, DateTimeKind.Unspecified).AddTicks(7647), new TimeSpan(0, 0, 0, 0, 0)) });
        }
    }
}

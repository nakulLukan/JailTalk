using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class IsReadonlyFieldAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_readonly",
                table: "application_settings",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "last_update_by",
                table: "application_settings",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "last_updated_on",
                table: "application_settings",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 1,
                columns: new[] { "is_readonly", "last_update_by", "last_updated_on" },
                values: new object[] { false, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "is_readonly", "last_update_by", "last_updated_on" },
                values: new object[] { false, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "is_readonly", "last_update_by", "last_updated_on" },
                values: new object[] { false, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "is_readonly", "last_update_by", "last_updated_on" },
                values: new object[] { false, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "is_readonly", "last_update_by", "last_updated_on" },
                values: new object[] { true, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "is_readonly", "last_update_by", "last_updated_on" },
                values: new object[] { true, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_readonly",
                table: "application_settings");

            migrationBuilder.DropColumn(
                name: "last_update_by",
                table: "application_settings");

            migrationBuilder.DropColumn(
                name: "last_updated_on",
                table: "application_settings");
        }
    }
}

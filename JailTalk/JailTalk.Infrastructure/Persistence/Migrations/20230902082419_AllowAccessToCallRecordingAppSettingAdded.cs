using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AllowAccessToCallRecordingAppSettingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "application_settings",
                columns: new[] { "id", "description", "is_readonly", "last_update_by", "last_updated_on", "regex_validation", "value" },
                values: new object[] { 7, "Allow user to access prisoners call recordings. Allowed Values: 'true' or 'false'", false, "System", new DateTimeOffset(new DateTime(2023, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^(true|false)$", "True" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 7);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CallTimeWindowApplicationSettingAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE prisoners CASCADE;");
            migrationBuilder.InsertData(
                table: "application_settings",
                columns: new[] { "id", "description", "is_readonly", "last_update_by", "last_updated_on", "regex_validation", "value" },
                values: new object[] { 8, "Time window during which the prisoners can access the telephone device and make calls.", false, "System", new DateTimeOffset(new DateTime(2023, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^[0-9]+[0-9]+[:][0-9]+[0-9]+[-][0-9]+[0-9]+[:][0-9]+[0-9]+$", "09:00-20:00" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 8);
        }
    }
}

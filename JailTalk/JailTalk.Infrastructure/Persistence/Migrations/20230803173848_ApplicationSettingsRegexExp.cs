using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationSettingsRegexExp : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "regex_validation",
                table: "application_settings",
                type: "character varying(100)",
                maxLength: 100,
                nullable: true);

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 1,
                column: "regex_validation",
                value: "^[0-9]+$");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 2,
                columns: new[] { "regex_validation", "value" },
                values: new object[] { "^[0-9]+$", "30" });

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 3,
                columns: new[] { "regex_validation", "value" },
                values: new object[] { "^[0-9]+$", "30" });

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 4,
                columns: new[] { "regex_validation", "value" },
                values: new object[] { "^[0-9]+$", "30" });

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 5,
                column: "regex_validation",
                value: "^[-+]?[0-9]*\\.?[0-9]+$");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 6,
                columns: new[] { "description", "regex_validation" },
                values: new object[] { "Maximum allowed call time per month in rupees. The prisoner cannot make any call of the total talktime (in minutes) amount in a month crosses this value.", "^[0-9]+$" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "regex_validation",
                table: "application_settings");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 2,
                column: "value",
                value: "15");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 3,
                column: "value",
                value: "20");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 4,
                column: "value",
                value: "15");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 6,
                column: "description",
                value: "Maximum allowed call time per month in rupees. The prisoner cannot make any call of the total talktime amount in a month crosses this value.");
        }
    }
}

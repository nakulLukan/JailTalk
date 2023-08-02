using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationSettingsNewAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 2,
                column: "description",
                value: "Max number of minutes a male prisoner can make a call a day.");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 3,
                column: "description",
                value: "Max number of minutes a female prisoner can make a call a day.");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 4,
                column: "description",
                value: "Max number of minutes a LGBTQ+ prisoner can make a call a day.");

            migrationBuilder.InsertData(
                table: "application_settings",
                columns: new[] { "id", "description", "value" },
                values: new object[,]
                {
                    { 5, "Amount charged for 1 minute call in rupees.", "0.5" },
                    { 6, "Maximum allowed call time per month in rupees. The prisoner cannot make any call of the total talktime amount in a month crosses this value.", "450" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 6);

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 2,
                column: "description",
                value: "Max number of minutes a male prisoner can make a call.");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 3,
                column: "description",
                value: "Max number of minutes a female prisoner can make a call.");

            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 4,
                column: "description",
                value: "Max number of minutes a LGBTQ+ prisoner can make a call.");
        }
    }
}

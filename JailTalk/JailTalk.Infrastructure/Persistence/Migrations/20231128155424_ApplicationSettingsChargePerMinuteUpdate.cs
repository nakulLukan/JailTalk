using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class ApplicationSettingsChargePerMinuteUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "description", "value" },
                values: new object[] { "Amount charged for 1 minute call in rupees. Minimum charge will be given 'Value' and after 1 minute projected 'Value' per second is charged for each second.", "1.5" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "application_settings",
                keyColumn: "id",
                keyValue: 5,
                columns: new[] { "description", "value" },
                values: new object[] { "Amount charged for 1 minute call in rupees.", "0.5" });
        }
    }
}

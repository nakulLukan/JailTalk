using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class EndOnNonMandatory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_application_setting",
                table: "application_setting");

            migrationBuilder.RenameTable(
                name: "application_setting",
                newName: "application_settings");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ended_on",
                table: "call_history",
                type: "timestamp with time zone",
                nullable: true,
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone");

            migrationBuilder.AddPrimaryKey(
                name: "pk_application_settings",
                table: "application_settings",
                column: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "pk_application_settings",
                table: "application_settings");

            migrationBuilder.RenameTable(
                name: "application_settings",
                newName: "application_setting");

            migrationBuilder.AlterColumn<DateTimeOffset>(
                name: "ended_on",
                table: "call_history",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                oldClrType: typeof(DateTimeOffset),
                oldType: "timestamp with time zone",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "pk_application_setting",
                table: "application_setting",
                column: "id");
        }
    }
}

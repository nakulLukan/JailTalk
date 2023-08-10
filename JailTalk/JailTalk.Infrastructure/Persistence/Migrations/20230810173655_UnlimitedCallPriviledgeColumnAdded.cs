using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class UnlimitedCallPriviledgeColumnAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoners_jails_jail_id",
                table: "prisoners");

            migrationBuilder.AlterColumn<int>(
                name: "jail_id",
                table: "prisoners",
                type: "integer",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "allow_unlimited_calls_till",
                table: "prisoners",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_system_generated_user",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldDefaultValue: true);

            migrationBuilder.AddForeignKey(
                name: "fk_prisoners_jails_jail_id",
                table: "prisoners",
                column: "jail_id",
                principalTable: "jails",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoners_jails_jail_id",
                table: "prisoners");

            migrationBuilder.DropColumn(
                name: "allow_unlimited_calls_till",
                table: "prisoners");

            migrationBuilder.AlterColumn<int>(
                name: "jail_id",
                table: "prisoners",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "is_system_generated_user",
                table: "AspNetUsers",
                type: "boolean",
                nullable: false,
                defaultValue: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddForeignKey(
                name: "fk_prisoners_jails_jail_id",
                table: "prisoners",
                column: "jail_id",
                principalTable: "jails",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

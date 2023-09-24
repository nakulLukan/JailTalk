using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class CallHistoryJailAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "associated_prison_id",
                table: "call_history",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "ix_call_history_associated_prison_id",
                table: "call_history",
                column: "associated_prison_id");

            migrationBuilder.AddForeignKey(
                name: "fk_call_history_jails_associated_prison_id",
                table: "call_history",
                column: "associated_prison_id",
                principalTable: "jails",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_call_history_jails_associated_prison_id",
                table: "call_history");

            migrationBuilder.DropIndex(
                name: "ix_call_history_associated_prison_id",
                table: "call_history");

            migrationBuilder.DropColumn(
                name: "associated_prison_id",
                table: "call_history");
        }
    }
}

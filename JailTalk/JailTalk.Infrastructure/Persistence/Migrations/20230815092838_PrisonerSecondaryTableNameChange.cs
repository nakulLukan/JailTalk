using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PrisonerSecondaryTableNameChange : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_function_prisoners_prisoner_id",
                table: "prisoner_function");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prisoner_function",
                table: "prisoner_function");

            migrationBuilder.RenameTable(
                name: "prisoner_function",
                newName: "prisoner_functions");

            migrationBuilder.RenameIndex(
                name: "ix_prisoner_function_prisoner_id",
                table: "prisoner_functions",
                newName: "ix_prisoner_functions_prisoner_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prisoner_functions",
                table: "prisoner_functions",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_functions_prisoners_prisoner_id",
                table: "prisoner_functions",
                column: "prisoner_id",
                principalTable: "prisoners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "fk_prisoner_functions_prisoners_prisoner_id",
                table: "prisoner_functions");

            migrationBuilder.DropPrimaryKey(
                name: "pk_prisoner_functions",
                table: "prisoner_functions");

            migrationBuilder.RenameTable(
                name: "prisoner_functions",
                newName: "prisoner_function");

            migrationBuilder.RenameIndex(
                name: "ix_prisoner_functions_prisoner_id",
                table: "prisoner_function",
                newName: "ix_prisoner_function_prisoner_id");

            migrationBuilder.AddPrimaryKey(
                name: "pk_prisoner_function",
                table: "prisoner_function",
                column: "id");

            migrationBuilder.AddForeignKey(
                name: "fk_prisoner_function_prisoners_prisoner_id",
                table: "prisoner_function",
                column: "prisoner_id",
                principalTable: "prisoners",
                principalColumn: "id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

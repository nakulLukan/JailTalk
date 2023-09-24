using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class PidNumber : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql("TRUNCATE TABLE prisoners CASCADE;");
            migrationBuilder.AddColumn<long>(
                name: "pid_number",
                table: "prisoners",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateIndex(
                name: "ix_prisoners_pid_number",
                table: "prisoners",
                column: "pid_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "ix_prisoners_pid_number",
                table: "prisoners");

            migrationBuilder.DropColumn(
                name: "pid_number",
                table: "prisoners");
        }
    }
}

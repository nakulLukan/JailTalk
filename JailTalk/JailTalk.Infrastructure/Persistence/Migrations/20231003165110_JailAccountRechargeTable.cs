using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class JailAccountRechargeTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "jail_account_balance",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    jail_id = table.Column<int>(type: "integer", nullable: false),
                    balance_amount = table.Column<float>(type: "real", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jail_account_balance", x => x.id);
                    table.ForeignKey(
                        name: "fk_jail_account_balance_jails_jail_id",
                        column: x => x.jail_id,
                        principalTable: "jails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "jail_account_recharge_requests",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    jail_id = table.Column<int>(type: "integer", nullable: false),
                    recharge_secret_hash = table.Column<string>(type: "character varying(128)", maxLength: 128, nullable: false),
                    recharge_amount = table.Column<float>(type: "real", nullable: false),
                    requested_by = table.Column<string>(type: "text", nullable: false),
                    requested_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    request_status = table.Column<short>(type: "smallint", nullable: false),
                    request_completed_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    expires_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    retry_count = table.Column<short>(type: "smallint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jail_account_recharge_requests", x => x.id);
                    table.ForeignKey(
                        name: "fk_jail_account_recharge_requests_jails_jail_id",
                        column: x => x.jail_id,
                        principalTable: "jails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "ix_jail_account_balance_jail_id",
                table: "jail_account_balance",
                column: "jail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_jail_account_recharge_requests_jail_id",
                table: "jail_account_recharge_requests",
                column: "jail_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "jail_account_balance");

            migrationBuilder.DropTable(
                name: "jail_account_recharge_requests");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "application_settings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false),
                    value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    regex_validation = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_readonly = table.Column<bool>(type: "boolean", nullable: false),
                    last_updated_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    last_update_by = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_application_settings", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoles",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    description = table.Column<string>(type: "text", nullable: true),
                    name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_roles", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "attachments",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    data = table.Column<byte[]>(type: "bytea", nullable: true),
                    relative_file_path = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    is_blob = table.Column<bool>(type: "boolean", nullable: false),
                    file_name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    file_size_in_bytes = table.Column<long>(type: "bigint", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachments", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "lookup_masters",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    internal_name = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lookup_masters", x => x.id);
                });

            migrationBuilder.CreateTable(
                name: "AspNetRoleClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    role_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_role_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_role_claims_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "lookup_details",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    lookup_master_id = table.Column<int>(type: "integer", nullable: false),
                    internal_name = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    value = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    order = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_lookup_details", x => x.id);
                    table.ForeignKey(
                        name: "fk_lookup_details_lookup_masters_lookup_master_id",
                        column: x => x.lookup_master_id,
                        principalTable: "lookup_masters",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "address_book",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    house_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    street = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    city = table.Column<string>(type: "character varying(30)", maxLength: 30, nullable: true),
                    state_id = table.Column<int>(type: "integer", nullable: true),
                    country_id = table.Column<int>(type: "integer", nullable: true),
                    pin_code = table.Column<string>(type: "character varying(10)", maxLength: 10, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_address_book", x => x.id);
                    table.ForeignKey(
                        name: "fk_address_book_lookup_details_country_id",
                        column: x => x.country_id,
                        principalTable: "lookup_details",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_address_book_lookup_details_state_id",
                        column: x => x.state_id,
                        principalTable: "lookup_details",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "jails",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    name = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    address_id = table.Column<long>(type: "bigint", nullable: true),
                    contact_email_address = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    is_system_turned_off = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_jails", x => x.id);
                    table.ForeignKey(
                        name: "fk_jails_address_book_address_id",
                        column: x => x.address_id,
                        principalTable: "address_book",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUsers",
                columns: table => new
                {
                    id = table.Column<string>(type: "text", nullable: false),
                    full_name = table.Column<string>(type: "text", nullable: true),
                    is_system_generated_user = table.Column<bool>(type: "boolean", nullable: false),
                    prison_id = table.Column<int>(type: "integer", nullable: true),
                    user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_user_name = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    normalized_email = table.Column<string>(type: "character varying(256)", maxLength: 256, nullable: true),
                    email_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    password_hash = table.Column<string>(type: "text", nullable: true),
                    security_stamp = table.Column<string>(type: "text", nullable: true),
                    concurrency_stamp = table.Column<string>(type: "text", nullable: true),
                    phone_number = table.Column<string>(type: "text", nullable: true),
                    phone_number_confirmed = table.Column<bool>(type: "boolean", nullable: false),
                    two_factor_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_enabled = table.Column<bool>(type: "boolean", nullable: false),
                    access_failed_count = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_users", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_users_jails_prison_id",
                        column: x => x.prison_id,
                        principalTable: "jails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "devices",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    code = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    device_secret_identifier = table.Column<Guid>(type: "uuid", nullable: false),
                    jail_id = table.Column<int>(type: "integer", nullable: false),
                    mac_address = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    failed_login_attempts = table.Column<int>(type: "integer", nullable: false),
                    last_logged_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    lockout_end = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_devices", x => x.id);
                    table.ForeignKey(
                        name: "fk_devices_jails_jail_id",
                        column: x => x.jail_id,
                        principalTable: "jails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateTable(
                name: "prisoners",
                columns: table => new
                {
                    id = table.Column<Guid>(type: "uuid", nullable: false),
                    pid_number = table.Column<long>(type: "bigint", nullable: false),
                    pid = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: false),
                    first_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    middle_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    last_name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    full_name = table.Column<string>(type: "character varying(150)", maxLength: 150, nullable: false),
                    jail_id = table.Column<int>(type: "integer", nullable: true),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    is_active = table.Column<bool>(type: "boolean", nullable: false, defaultValue: true),
                    gender = table.Column<int>(type: "integer", nullable: false, defaultValue: 1),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prisoners", x => x.id);
                    table.ForeignKey(
                        name: "fk_prisoners_jails_jail_id",
                        column: x => x.jail_id,
                        principalTable: "jails",
                        principalColumn: "id");
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserClaims",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    user_id = table.Column<string>(type: "text", nullable: false),
                    claim_type = table.Column<string>(type: "text", nullable: true),
                    claim_value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_claims", x => x.id);
                    table.ForeignKey(
                        name: "fk_asp_net_user_claims_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserLogins",
                columns: table => new
                {
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    provider_key = table.Column<string>(type: "text", nullable: false),
                    provider_display_name = table.Column<string>(type: "text", nullable: true),
                    user_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_logins", x => new { x.login_provider, x.provider_key });
                    table.ForeignKey(
                        name: "fk_asp_net_user_logins_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserRoles",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    role_id = table.Column<string>(type: "text", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_roles", x => new { x.user_id, x.role_id });
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_roles_role_id",
                        column: x => x.role_id,
                        principalTable: "AspNetRoles",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_asp_net_user_roles_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AspNetUserTokens",
                columns: table => new
                {
                    user_id = table.Column<string>(type: "text", nullable: false),
                    login_provider = table.Column<string>(type: "text", nullable: false),
                    name = table.Column<string>(type: "text", nullable: false),
                    value = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_asp_net_user_tokens", x => new { x.user_id, x.login_provider, x.name });
                    table.ForeignKey(
                        name: "fk_asp_net_user_tokens_asp_net_users_user_id",
                        column: x => x.user_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phone_balances",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prisoner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    balance = table.Column<float>(type: "real", nullable: false),
                    last_updated_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_phone_balances", x => x.id);
                    table.ForeignKey(
                        name: "fk_phone_balances_prisoners_prisoner_id",
                        column: x => x.prisoner_id,
                        principalTable: "prisoners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phone_directory",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prisoner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    phone_number = table.Column<string>(type: "character varying(15)", maxLength: 15, nullable: false),
                    country_code = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    name = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_active = table.Column<bool>(type: "boolean", nullable: false),
                    is_blocked = table.Column<bool>(type: "boolean", nullable: false),
                    last_blocked_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    relative_type_id = table.Column<int>(type: "integer", nullable: false),
                    relative_address_id = table.Column<long>(type: "bigint", nullable: false),
                    id_proof_type_id = table.Column<int>(type: "integer", nullable: true),
                    id_proof_value = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    is_call_recording_allowed = table.Column<bool>(type: "boolean", nullable: false),
                    created_by = table.Column<string>(type: "text", nullable: true),
                    updated_by = table.Column<string>(type: "text", nullable: true),
                    created_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    updated_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_phone_directory", x => x.id);
                    table.ForeignKey(
                        name: "fk_phone_directory_address_book_relative_address_id",
                        column: x => x.relative_address_id,
                        principalTable: "address_book",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_phone_directory_lookup_details_id_proof_type_id",
                        column: x => x.id_proof_type_id,
                        principalTable: "lookup_details",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_phone_directory_lookup_details_relative_type_id",
                        column: x => x.relative_type_id,
                        principalTable: "lookup_details",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_phone_directory_prisoners_prisoner_id",
                        column: x => x.prisoner_id,
                        principalTable: "prisoners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prisoner_face_encoding_mappings",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prisoner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    image_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prisoner_face_encoding_mappings", x => x.id);
                    table.ForeignKey(
                        name: "fk_prisoner_face_encoding_mappings_attachments_image_id",
                        column: x => x.image_id,
                        principalTable: "attachments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_prisoner_face_encoding_mappings_prisoners_prisoner_id",
                        column: x => x.prisoner_id,
                        principalTable: "prisoners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "prisoner_functions",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    prisoner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    last_associated_jail_id = table.Column<int>(type: "integer", nullable: true),
                    last_released_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    punishment_ends_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    unlimited_calls_ends_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    dp_attachment_id = table.Column<int>(type: "integer", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_prisoner_functions", x => x.id);
                    table.ForeignKey(
                        name: "fk_prisoner_functions_attachments_dp_attachment_id",
                        column: x => x.dp_attachment_id,
                        principalTable: "attachments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_prisoner_functions_jails_last_associated_jail_id",
                        column: x => x.last_associated_jail_id,
                        principalTable: "jails",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_prisoner_functions_prisoners_prisoner_id",
                        column: x => x.prisoner_id,
                        principalTable: "prisoners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "attachment_phone_directory",
                columns: table => new
                {
                    id_proof_attachments_id = table.Column<int>(type: "integer", nullable: false),
                    phone_directory_id = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_attachment_phone_directory", x => new { x.id_proof_attachments_id, x.phone_directory_id });
                    table.ForeignKey(
                        name: "fk_attachment_phone_directory_attachments_id_proof_attachments",
                        column: x => x.id_proof_attachments_id,
                        principalTable: "attachments",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_attachment_phone_directory_phone_directory_phone_directory_",
                        column: x => x.phone_directory_id,
                        principalTable: "phone_directory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "call_history",
                columns: table => new
                {
                    id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    phone_directory_id = table.Column<long>(type: "bigint", nullable: false),
                    call_started_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ended_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: true),
                    call_termination_reason = table.Column<short>(type: "smallint", nullable: false),
                    call_recording_attachment_id = table.Column<int>(type: "integer", nullable: true),
                    associated_prison_id = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_call_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_call_history_attachments_call_recording_attachment_id",
                        column: x => x.call_recording_attachment_id,
                        principalTable: "attachments",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_call_history_jails_associated_prison_id",
                        column: x => x.associated_prison_id,
                        principalTable: "jails",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "fk_call_history_phone_directory_phone_directory_id",
                        column: x => x.phone_directory_id,
                        principalTable: "phone_directory",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "phone_balance_history",
                columns: table => new
                {
                    id = table.Column<int>(type: "integer", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    net_amount = table.Column<float>(type: "real", nullable: false),
                    amount_difference = table.Column<float>(type: "real", nullable: false),
                    reason = table.Column<short>(type: "smallint", nullable: false),
                    prisoner_id = table.Column<Guid>(type: "uuid", nullable: false),
                    created_on = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    call_request_id = table.Column<long>(type: "bigint", nullable: true),
                    recharged_by_user_id = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("pk_phone_balance_history", x => x.id);
                    table.ForeignKey(
                        name: "fk_phone_balance_history_call_history_call_request_id",
                        column: x => x.call_request_id,
                        principalTable: "call_history",
                        principalColumn: "id");
                    table.ForeignKey(
                        name: "fk_phone_balance_history_prisoners_prisoner_id",
                        column: x => x.prisoner_id,
                        principalTable: "prisoners",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "application_settings",
                columns: new[] { "id", "description", "is_readonly", "last_update_by", "last_updated_on", "regex_validation", "value" },
                values: new object[,]
                {
                    { 1, "Max number of contacts to show to a prisoner", false, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^[0-9]+$", "3" },
                    { 2, "Max number of minutes a male prisoner can make a call a day.", false, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^[0-9]+$", "30" },
                    { 3, "Max number of minutes a female prisoner can make a call a day.", false, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^[0-9]+$", "30" },
                    { 4, "Max number of minutes a LGBTQ+ prisoner can make a call a day.", false, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^[0-9]+$", "30" },
                    { 5, "Amount charged for 1 minute call in rupees. Minimum charge will be given 'Value' and after 1 minute projected 'Value' per second is charged for each second.", true, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^[-+]?[0-9]*\\.?[0-9]+$", "1.5" },
                    { 6, "Maximum allowed call time per month in rupees. The prisoner cannot make any call of the total talktime (in minutes) amount in a month crosses this value.", true, "System", new DateTimeOffset(new DateTime(2023, 5, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^[0-9]+$", "450" },
                    { 7, "Allow user to access prisoners call recordings. Allowed Values: 'true' or 'false'", false, "System", new DateTimeOffset(new DateTime(2023, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^(true|false)$", "True" },
                    { 8, "Time window during which the prisoners can access the telephone device and make calls.", false, "System", new DateTimeOffset(new DateTime(2023, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), "^[0-9]+[0-9]+[:][0-9]+[0-9]+[-][0-9]+[0-9]+[:][0-9]+[0-9]+$", "09:00-20:00" }
                });

            migrationBuilder.InsertData(
                table: "jails",
                columns: new[] { "id", "address_id", "code", "contact_email_address", "created_by", "created_on", "is_system_turned_off", "name", "updated_by", "updated_on" },
                values: new object[] { 1, null, "MLP-PN-SJ", null, null, new DateTimeOffset(new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), false, "Ponnani Sub Jail", null, new DateTimeOffset(new DateTime(2023, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)) });

            migrationBuilder.InsertData(
                table: "lookup_masters",
                columns: new[] { "id", "internal_name", "is_active", "name" },
                values: new object[,]
                {
                    { 1, "State", true, "States" },
                    { 2, "Country", true, "Countries" },
                    { 3, "Relationship", true, "Relationship" },
                    { 4, "IdProof", true, "ID Proof" }
                });

            migrationBuilder.InsertData(
                table: "lookup_details",
                columns: new[] { "id", "internal_name", "is_active", "lookup_master_id", "order", "value" },
                values: new object[,]
                {
                    { 1, "kerala", true, 1, 1, "Kerala" },
                    { 2, "india", true, 2, 1, "India" },
                    { 3, "father", true, 3, 1, "Father" },
                    { 4, "mother", true, 3, 2, "Mother" },
                    { 5, "brother", true, 3, 3, "Brother" },
                    { 6, "sister", true, 3, 4, "Sister" },
                    { 7, "lawyer", true, 3, 5, "Lawyer" },
                    { 8, "other", true, 3, 6, "Others" },
                    { 9, "aadhar", true, 4, 1, "Aadhar" },
                    { 10, "driving_license", true, 4, 2, "Driving License" }
                });

            migrationBuilder.CreateIndex(
                name: "ix_address_book_country_id",
                table: "address_book",
                column: "country_id");

            migrationBuilder.CreateIndex(
                name: "ix_address_book_state_id",
                table: "address_book",
                column: "state_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_role_claims_role_id",
                table: "AspNetRoleClaims",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "RoleNameIndex",
                table: "AspNetRoles",
                column: "normalized_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_claims_user_id",
                table: "AspNetUserClaims",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_logins_user_id",
                table: "AspNetUserLogins",
                column: "user_id");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_user_roles_role_id",
                table: "AspNetUserRoles",
                column: "role_id");

            migrationBuilder.CreateIndex(
                name: "EmailIndex",
                table: "AspNetUsers",
                column: "normalized_email");

            migrationBuilder.CreateIndex(
                name: "ix_asp_net_users_prison_id",
                table: "AspNetUsers",
                column: "prison_id");

            migrationBuilder.CreateIndex(
                name: "UserNameIndex",
                table: "AspNetUsers",
                column: "normalized_user_name",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_attachment_phone_directory_phone_directory_id",
                table: "attachment_phone_directory",
                column: "phone_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_call_history_associated_prison_id",
                table: "call_history",
                column: "associated_prison_id");

            migrationBuilder.CreateIndex(
                name: "ix_call_history_call_recording_attachment_id",
                table: "call_history",
                column: "call_recording_attachment_id");

            migrationBuilder.CreateIndex(
                name: "ix_call_history_phone_directory_id",
                table: "call_history",
                column: "phone_directory_id");

            migrationBuilder.CreateIndex(
                name: "ix_devices_jail_id",
                table: "devices",
                column: "jail_id");

            migrationBuilder.CreateIndex(
                name: "ix_devices_mac_address",
                table: "devices",
                column: "mac_address");

            migrationBuilder.CreateIndex(
                name: "ix_jail_account_balance_jail_id",
                table: "jail_account_balance",
                column: "jail_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_jail_account_recharge_requests_jail_id",
                table: "jail_account_recharge_requests",
                column: "jail_id");

            migrationBuilder.CreateIndex(
                name: "ix_jails_address_id",
                table: "jails",
                column: "address_id");

            migrationBuilder.CreateIndex(
                name: "ix_lookup_details_lookup_master_id",
                table: "lookup_details",
                column: "lookup_master_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_balance_history_call_request_id",
                table: "phone_balance_history",
                column: "call_request_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_phone_balance_history_prisoner_id",
                table: "phone_balance_history",
                column: "prisoner_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_balances_prisoner_id",
                table: "phone_balances",
                column: "prisoner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_phone_directory_id_proof_type_id",
                table: "phone_directory",
                column: "id_proof_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_directory_prisoner_id",
                table: "phone_directory",
                column: "prisoner_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_directory_relative_address_id",
                table: "phone_directory",
                column: "relative_address_id");

            migrationBuilder.CreateIndex(
                name: "ix_phone_directory_relative_type_id",
                table: "phone_directory",
                column: "relative_type_id");

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_face_encoding_mappings_image_id",
                table: "prisoner_face_encoding_mappings",
                column: "image_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_face_encoding_mappings_prisoner_id",
                table: "prisoner_face_encoding_mappings",
                column: "prisoner_id");

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_functions_dp_attachment_id",
                table: "prisoner_functions",
                column: "dp_attachment_id");

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_functions_last_associated_jail_id",
                table: "prisoner_functions",
                column: "last_associated_jail_id");

            migrationBuilder.CreateIndex(
                name: "ix_prisoner_functions_prisoner_id",
                table: "prisoner_functions",
                column: "prisoner_id",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "ix_prisoners_jail_id",
                table: "prisoners",
                column: "jail_id");

            migrationBuilder.CreateIndex(
                name: "ix_prisoners_pid",
                table: "prisoners",
                column: "pid");

            migrationBuilder.CreateIndex(
                name: "ix_prisoners_pid_number",
                table: "prisoners",
                column: "pid_number");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "application_settings");

            migrationBuilder.DropTable(
                name: "AspNetRoleClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserClaims");

            migrationBuilder.DropTable(
                name: "AspNetUserLogins");

            migrationBuilder.DropTable(
                name: "AspNetUserRoles");

            migrationBuilder.DropTable(
                name: "AspNetUserTokens");

            migrationBuilder.DropTable(
                name: "attachment_phone_directory");

            migrationBuilder.DropTable(
                name: "devices");

            migrationBuilder.DropTable(
                name: "jail_account_balance");

            migrationBuilder.DropTable(
                name: "jail_account_recharge_requests");

            migrationBuilder.DropTable(
                name: "phone_balance_history");

            migrationBuilder.DropTable(
                name: "phone_balances");

            migrationBuilder.DropTable(
                name: "prisoner_face_encoding_mappings");

            migrationBuilder.DropTable(
                name: "prisoner_functions");

            migrationBuilder.DropTable(
                name: "AspNetRoles");

            migrationBuilder.DropTable(
                name: "AspNetUsers");

            migrationBuilder.DropTable(
                name: "call_history");

            migrationBuilder.DropTable(
                name: "attachments");

            migrationBuilder.DropTable(
                name: "phone_directory");

            migrationBuilder.DropTable(
                name: "prisoners");

            migrationBuilder.DropTable(
                name: "jails");

            migrationBuilder.DropTable(
                name: "address_book");

            migrationBuilder.DropTable(
                name: "lookup_details");

            migrationBuilder.DropTable(
                name: "lookup_masters");
        }
    }
}

﻿// <auto-generated />
using System;
using JailTalk.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "7.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 63);

            NpgsqlModelBuilderExtensions.UseIdentityByDefaultColumns(modelBuilder);

            modelBuilder.Entity("JailTalk.Domain.Identity.AppUser", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<int>("AccessFailedCount")
                        .HasColumnType("integer")
                        .HasColumnName("access_failed_count");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Email")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("email");

                    b.Property<bool>("EmailConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("email_confirmed");

                    b.Property<bool>("LockoutEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("lockout_enabled");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("NormalizedEmail")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_email");

                    b.Property<string>("NormalizedUserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_user_name");

                    b.Property<string>("PasswordHash")
                        .HasColumnType("text")
                        .HasColumnName("password_hash");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("text")
                        .HasColumnName("phone_number");

                    b.Property<bool>("PhoneNumberConfirmed")
                        .HasColumnType("boolean")
                        .HasColumnName("phone_number_confirmed");

                    b.Property<string>("SecurityStamp")
                        .HasColumnType("text")
                        .HasColumnName("security_stamp");

                    b.Property<bool>("TwoFactorEnabled")
                        .HasColumnType("boolean")
                        .HasColumnName("two_factor_enabled");

                    b.Property<string>("UserName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("user_name");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_users");

                    b.HasIndex("NormalizedEmail")
                        .HasDatabaseName("EmailIndex");

                    b.HasIndex("NormalizedUserName")
                        .IsUnique()
                        .HasDatabaseName("UserNameIndex");

                    b.ToTable("AspNetUsers", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Lookup.AddressBook", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("City")
                        .HasMaxLength(30)
                        .HasColumnType("character varying(30)")
                        .HasColumnName("city");

                    b.Property<int?>("CountryId")
                        .HasColumnType("integer")
                        .HasColumnName("country_id");

                    b.Property<string>("HouseName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("house_name");

                    b.Property<int?>("StateId")
                        .HasColumnType("integer")
                        .HasColumnName("state_id");

                    b.Property<string>("Street")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("street");

                    b.HasKey("Id")
                        .HasName("pk_address_book");

                    b.HasIndex("CountryId")
                        .HasDatabaseName("ix_address_book_country_id");

                    b.HasIndex("StateId")
                        .HasDatabaseName("ix_address_book_state_id");

                    b.ToTable("address_book", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Lookup.LookupDetail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasMaxLength(20)
                        .HasColumnType("character varying(20)")
                        .HasColumnName("internal_name");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<int>("LookupMasterId")
                        .HasColumnType("integer")
                        .HasColumnName("lookup_master_id");

                    b.Property<int?>("Order")
                        .HasColumnType("integer")
                        .HasColumnName("order");

                    b.Property<string>("Value")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("value");

                    b.HasKey("Id")
                        .HasName("pk_lookup_details");

                    b.HasIndex("LookupMasterId")
                        .HasDatabaseName("ix_lookup_details_lookup_master_id");

                    b.ToTable("lookup_details", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Lookup.LookupMaster", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("InternalName")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("internal_name");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("name");

                    b.HasKey("Id")
                        .HasName("pk_lookup_masters");

                    b.ToTable("lookup_masters", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.CallHistory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<DateTimeOffset>("CallStartedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("call_started_on");

                    b.Property<short>("CallTerminationReason")
                        .HasColumnType("smallint")
                        .HasColumnName("call_termination_reason");

                    b.Property<DateTimeOffset>("EndedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("ended_on");

                    b.Property<long>("PhoneDirectoryId")
                        .HasColumnType("bigint")
                        .HasColumnName("phone_directory_id");

                    b.HasKey("Id")
                        .HasName("pk_call_history");

                    b.HasIndex("PhoneDirectoryId")
                        .HasDatabaseName("ix_call_history_phone_directory_id");

                    b.ToTable("call_history", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.Device", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("code");

                    b.Property<Guid>("DeviceSecretIdentifier")
                        .HasColumnType("uuid")
                        .HasColumnName("device_secret_identifier");

                    b.Property<int>("FailedLoginAttempts")
                        .HasColumnType("integer")
                        .HasColumnName("failed_login_attempts");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<int>("JailId")
                        .HasColumnType("integer")
                        .HasColumnName("jail_id");

                    b.Property<DateTimeOffset?>("LastLoggedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_logged_on");

                    b.Property<DateTimeOffset?>("LockoutEnd")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("lockout_end");

                    b.Property<string>("MacAddress")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("mac_address");

                    b.HasKey("Id")
                        .HasName("pk_devices");

                    b.HasIndex("JailId")
                        .HasDatabaseName("ix_devices_jail_id");

                    b.HasIndex("MacAddress")
                        .HasDatabaseName("ix_devices_mac_address");

                    b.ToTable("devices", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.Jail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<long?>("AddressId")
                        .HasColumnType("bigint")
                        .HasColumnName("address_id");

                    b.Property<string>("Code")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("code");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("Name")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("name");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on");

                    b.HasKey("Id")
                        .HasName("pk_jails");

                    b.HasIndex("AddressId")
                        .HasDatabaseName("ix_jails_address_id");

                    b.ToTable("jails", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.PhoneBalance", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("Balance")
                        .HasColumnType("real")
                        .HasColumnName("balance");

                    b.Property<DateTimeOffset>("LastUpdatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_updated_on");

                    b.Property<Guid>("PrisonerId")
                        .HasColumnType("uuid")
                        .HasColumnName("prisoner_id");

                    b.HasKey("Id")
                        .HasName("pk_phone_balances");

                    b.HasIndex("PrisonerId")
                        .IsUnique()
                        .HasDatabaseName("ix_phone_balances_prisoner_id");

                    b.ToTable("phone_balances", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.PhoneBalanceHistory", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<float>("AmountDifference")
                        .HasColumnType("real")
                        .HasColumnName("amount_difference");

                    b.Property<long?>("CallRequestId")
                        .HasColumnType("bigint")
                        .HasColumnName("call_request_id");

                    b.Property<DateTimeOffset>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<float>("NetAmount")
                        .HasColumnType("real")
                        .HasColumnName("net_amount");

                    b.Property<Guid>("PrisonerId")
                        .HasColumnType("uuid")
                        .HasColumnName("prisoner_id");

                    b.Property<short>("Reason")
                        .HasColumnType("smallint")
                        .HasColumnName("reason");

                    b.Property<string>("RechargedByUserId")
                        .HasColumnType("text")
                        .HasColumnName("recharged_by_user_id");

                    b.HasKey("Id")
                        .HasName("pk_phone_balance_history");

                    b.HasIndex("CallRequestId")
                        .IsUnique()
                        .HasDatabaseName("ix_phone_balance_history_call_request_id");

                    b.HasIndex("PrisonerId")
                        .HasDatabaseName("ix_phone_balance_history_prisoner_id");

                    b.ToTable("phone_balance_history", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.PhoneDirectory", b =>
                {
                    b.Property<long>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("bigint")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<long>("Id"));

                    b.Property<string>("CountryCode")
                        .IsRequired()
                        .HasMaxLength(5)
                        .HasColumnType("character varying(5)")
                        .HasColumnName("country_code");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<bool>("IsActive")
                        .HasColumnType("boolean")
                        .HasColumnName("is_active");

                    b.Property<bool>("IsBlocked")
                        .HasColumnType("boolean")
                        .HasColumnName("is_blocked");

                    b.Property<DateTimeOffset?>("LastBlockedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("last_blocked_on");

                    b.Property<string>("PhoneNumber")
                        .IsRequired()
                        .HasMaxLength(15)
                        .HasColumnType("character varying(15)")
                        .HasColumnName("phone_number");

                    b.Property<Guid>("PrisonerId")
                        .HasColumnType("uuid")
                        .HasColumnName("prisoner_id");

                    b.Property<long>("RelativeAddressId")
                        .HasColumnType("bigint")
                        .HasColumnName("relative_address_id");

                    b.Property<int>("RelativeTypeId")
                        .HasColumnType("integer")
                        .HasColumnName("relative_type_id");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on");

                    b.HasKey("Id")
                        .HasName("pk_phone_directory");

                    b.HasIndex("PrisonerId")
                        .HasDatabaseName("ix_phone_directory_prisoner_id");

                    b.HasIndex("RelativeAddressId")
                        .HasDatabaseName("ix_phone_directory_relative_address_id");

                    b.HasIndex("RelativeTypeId")
                        .HasDatabaseName("ix_phone_directory_relative_type_id");

                    b.ToTable("phone_directory", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.Prisoner", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uuid")
                        .HasColumnName("id");

                    b.Property<long?>("AddressId")
                        .HasColumnType("bigint")
                        .HasColumnName("address_id");

                    b.Property<string>("CreatedBy")
                        .HasColumnType("text")
                        .HasColumnName("created_by");

                    b.Property<DateTimeOffset?>("CreatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("created_on");

                    b.Property<string>("FirstName")
                        .IsRequired()
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("first_name");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasMaxLength(150)
                        .HasColumnType("character varying(150)")
                        .HasColumnName("full_name");

                    b.Property<int>("JailId")
                        .HasColumnType("integer")
                        .HasColumnName("jail_id");

                    b.Property<string>("LastName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("last_name");

                    b.Property<string>("MiddleName")
                        .HasMaxLength(50)
                        .HasColumnType("character varying(50)")
                        .HasColumnName("middle_name");

                    b.Property<string>("Pid")
                        .IsRequired()
                        .HasMaxLength(100)
                        .HasColumnType("character varying(100)")
                        .HasColumnName("pid");

                    b.Property<string>("UpdatedBy")
                        .HasColumnType("text")
                        .HasColumnName("updated_by");

                    b.Property<DateTimeOffset?>("UpdatedOn")
                        .HasColumnType("timestamp with time zone")
                        .HasColumnName("updated_on");

                    b.HasKey("Id")
                        .HasName("pk_prisoners");

                    b.HasIndex("AddressId")
                        .HasDatabaseName("ix_prisoners_address_id");

                    b.HasIndex("JailId")
                        .HasDatabaseName("ix_prisoners_jail_id");

                    b.HasIndex("Pid")
                        .HasDatabaseName("ix_prisoners_pid");

                    b.ToTable("prisoners", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRole", b =>
                {
                    b.Property<string>("Id")
                        .HasColumnType("text")
                        .HasColumnName("id");

                    b.Property<string>("ConcurrencyStamp")
                        .IsConcurrencyToken()
                        .HasColumnType("text")
                        .HasColumnName("concurrency_stamp");

                    b.Property<string>("Name")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("name");

                    b.Property<string>("NormalizedName")
                        .HasMaxLength(256)
                        .HasColumnType("character varying(256)")
                        .HasColumnName("normalized_name");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_roles");

                    b.HasIndex("NormalizedName")
                        .IsUnique()
                        .HasDatabaseName("RoleNameIndex");

                    b.ToTable("AspNetRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<string>("RoleId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_role_claims");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_role_claims_role_id");

                    b.ToTable("AspNetRoleClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("integer")
                        .HasColumnName("id");

                    NpgsqlPropertyBuilderExtensions.UseIdentityByDefaultColumn(b.Property<int>("Id"));

                    b.Property<string>("ClaimType")
                        .HasColumnType("text")
                        .HasColumnName("claim_type");

                    b.Property<string>("ClaimValue")
                        .HasColumnType("text")
                        .HasColumnName("claim_value");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("Id")
                        .HasName("pk_asp_net_user_claims");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_claims_user_id");

                    b.ToTable("AspNetUserClaims", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("ProviderKey")
                        .HasColumnType("text")
                        .HasColumnName("provider_key");

                    b.Property<string>("ProviderDisplayName")
                        .HasColumnType("text")
                        .HasColumnName("provider_display_name");

                    b.Property<string>("UserId")
                        .IsRequired()
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.HasKey("LoginProvider", "ProviderKey")
                        .HasName("pk_asp_net_user_logins");

                    b.HasIndex("UserId")
                        .HasDatabaseName("ix_asp_net_user_logins_user_id");

                    b.ToTable("AspNetUserLogins", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("RoleId")
                        .HasColumnType("text")
                        .HasColumnName("role_id");

                    b.HasKey("UserId", "RoleId")
                        .HasName("pk_asp_net_user_roles");

                    b.HasIndex("RoleId")
                        .HasDatabaseName("ix_asp_net_user_roles_role_id");

                    b.ToTable("AspNetUserRoles", (string)null);
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.Property<string>("UserId")
                        .HasColumnType("text")
                        .HasColumnName("user_id");

                    b.Property<string>("LoginProvider")
                        .HasColumnType("text")
                        .HasColumnName("login_provider");

                    b.Property<string>("Name")
                        .HasColumnType("text")
                        .HasColumnName("name");

                    b.Property<string>("Value")
                        .HasColumnType("text")
                        .HasColumnName("value");

                    b.HasKey("UserId", "LoginProvider", "Name")
                        .HasName("pk_asp_net_user_tokens");

                    b.ToTable("AspNetUserTokens", (string)null);
                });

            modelBuilder.Entity("JailTalk.Domain.Lookup.AddressBook", b =>
                {
                    b.HasOne("JailTalk.Domain.Lookup.LookupDetail", "Country")
                        .WithMany()
                        .HasForeignKey("CountryId")
                        .HasConstraintName("fk_address_book_lookup_details_country_id");

                    b.HasOne("JailTalk.Domain.Lookup.LookupDetail", "State")
                        .WithMany()
                        .HasForeignKey("StateId")
                        .HasConstraintName("fk_address_book_lookup_details_state_id");

                    b.Navigation("Country");

                    b.Navigation("State");
                });

            modelBuilder.Entity("JailTalk.Domain.Lookup.LookupDetail", b =>
                {
                    b.HasOne("JailTalk.Domain.Lookup.LookupMaster", "LookupMaster")
                        .WithMany("LookupDetails")
                        .HasForeignKey("LookupMasterId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_lookup_details_lookup_masters_lookup_master_id");

                    b.Navigation("LookupMaster");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.CallHistory", b =>
                {
                    b.HasOne("JailTalk.Domain.Prison.PhoneDirectory", "PhoneDirectory")
                        .WithMany("CallHistory")
                        .HasForeignKey("PhoneDirectoryId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_call_history_phone_directory_phone_directory_id");

                    b.Navigation("PhoneDirectory");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.Device", b =>
                {
                    b.HasOne("JailTalk.Domain.Prison.Jail", "Jail")
                        .WithMany()
                        .HasForeignKey("JailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_devices_jails_jail_id");

                    b.Navigation("Jail");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.Jail", b =>
                {
                    b.HasOne("JailTalk.Domain.Lookup.AddressBook", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .HasConstraintName("fk_jails_address_book_address_id");

                    b.Navigation("Address");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.PhoneBalance", b =>
                {
                    b.HasOne("JailTalk.Domain.Prison.Prisoner", "Prisoner")
                        .WithOne("PhoneBalance")
                        .HasForeignKey("JailTalk.Domain.Prison.PhoneBalance", "PrisonerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_phone_balances_prisoners_prisoner_id");

                    b.Navigation("Prisoner");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.PhoneBalanceHistory", b =>
                {
                    b.HasOne("JailTalk.Domain.Prison.CallHistory", "CallRequest")
                        .WithOne()
                        .HasForeignKey("JailTalk.Domain.Prison.PhoneBalanceHistory", "CallRequestId")
                        .HasConstraintName("fk_phone_balance_history_call_history_call_request_id");

                    b.HasOne("JailTalk.Domain.Prison.Prisoner", "Prisoner")
                        .WithMany()
                        .HasForeignKey("PrisonerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_phone_balance_history_prisoners_prisoner_id");

                    b.Navigation("CallRequest");

                    b.Navigation("Prisoner");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.PhoneDirectory", b =>
                {
                    b.HasOne("JailTalk.Domain.Prison.Prisoner", "Prisoner")
                        .WithMany("PhoneDirectory")
                        .HasForeignKey("PrisonerId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_phone_directory_prisoners_prisoner_id");

                    b.HasOne("JailTalk.Domain.Lookup.AddressBook", "RelativeAddress")
                        .WithMany()
                        .HasForeignKey("RelativeAddressId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_phone_directory_address_book_relative_address_id");

                    b.HasOne("JailTalk.Domain.Lookup.LookupDetail", "RelativeType")
                        .WithMany()
                        .HasForeignKey("RelativeTypeId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_phone_directory_lookup_details_relative_type_id");

                    b.Navigation("Prisoner");

                    b.Navigation("RelativeAddress");

                    b.Navigation("RelativeType");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.Prisoner", b =>
                {
                    b.HasOne("JailTalk.Domain.Lookup.AddressBook", "Address")
                        .WithMany()
                        .HasForeignKey("AddressId")
                        .HasConstraintName("fk_prisoners_address_book_address_id");

                    b.HasOne("JailTalk.Domain.Prison.Jail", "Jail")
                        .WithMany()
                        .HasForeignKey("JailId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_prisoners_jails_jail_id");

                    b.Navigation("Address");

                    b.Navigation("Jail");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityRoleClaim<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_role_claims_asp_net_roles_role_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserClaim<string>", b =>
                {
                    b.HasOne("JailTalk.Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_claims_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserLogin<string>", b =>
                {
                    b.HasOne("JailTalk.Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_logins_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserRole<string>", b =>
                {
                    b.HasOne("Microsoft.AspNetCore.Identity.IdentityRole", null)
                        .WithMany()
                        .HasForeignKey("RoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_asp_net_roles_role_id");

                    b.HasOne("JailTalk.Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_roles_asp_net_users_user_id");
                });

            modelBuilder.Entity("Microsoft.AspNetCore.Identity.IdentityUserToken<string>", b =>
                {
                    b.HasOne("JailTalk.Domain.Identity.AppUser", null)
                        .WithMany()
                        .HasForeignKey("UserId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired()
                        .HasConstraintName("fk_asp_net_user_tokens_asp_net_users_user_id");
                });

            modelBuilder.Entity("JailTalk.Domain.Lookup.LookupMaster", b =>
                {
                    b.Navigation("LookupDetails");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.PhoneDirectory", b =>
                {
                    b.Navigation("CallHistory");
                });

            modelBuilder.Entity("JailTalk.Domain.Prison.Prisoner", b =>
                {
                    b.Navigation("PhoneBalance");

                    b.Navigation("PhoneDirectory");
                });
#pragma warning restore 612, 618
        }
    }
}

﻿using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JailTalk.Infrastructure.Persistence.Migrations
{
    /// <inheritdoc />
    public partial class AddCallRecordingEnabledFeature : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "is_call_recording_allowed",
                table: "phone_directory",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "is_call_recording_allowed",
                table: "phone_directory");
        }
    }
}

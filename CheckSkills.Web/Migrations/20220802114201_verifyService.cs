using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckSkills.Web.Migrations
{
    public partial class verifyService : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Token",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "TokenCreated",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "PasswordResetToken",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ResetTokenExpires",
                table: "Students",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "VerificationToken",
                table: "Students",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "VerifiedAt",
                table: "Students",
                type: "datetime2",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "PasswordResetToken",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ResetTokenExpires",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "VerificationToken",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "VerifiedAt",
                table: "Students");

            migrationBuilder.AddColumn<string>(
                name: "Token",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenCreated",
                table: "Students",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckSkills.Web.Migrations
{
    public partial class fixToken : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_refreeshTokens",
                table: "refreeshTokens");

            migrationBuilder.RenameTable(
                name: "refreeshTokens",
                newName: "RefreshTokens");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Students",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Students",
                type: "varbinary(max)",
                nullable: true,
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens",
                column: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_RefreshTokens",
                table: "RefreshTokens");

            migrationBuilder.RenameTable(
                name: "RefreshTokens",
                newName: "refreeshTokens");

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordSalt",
                table: "Students",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AlterColumn<byte[]>(
                name: "PasswordHash",
                table: "Students",
                type: "varbinary(max)",
                nullable: false,
                defaultValue: new byte[0],
                oldClrType: typeof(byte[]),
                oldType: "varbinary(max)",
                oldNullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_refreeshTokens",
                table: "refreeshTokens",
                column: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckSkills.Web.Migrations
{
    public partial class CheckServices : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "StudentId",
                table: "Students",
                newName: "Id");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Students",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Email",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Students",
                newName: "StudentId");
        }
    }
}

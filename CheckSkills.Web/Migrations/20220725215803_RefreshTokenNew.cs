using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CheckSkills.Web.Migrations
{
    public partial class RefreshTokenNew : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_refreeshTokens_Students_StudentId",
                table: "refreeshTokens");

            migrationBuilder.DropIndex(
                name: "IX_refreeshTokens_StudentId",
                table: "refreeshTokens");

            migrationBuilder.DropColumn(
                name: "StudentId",
                table: "refreeshTokens");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StudentId",
                table: "refreeshTokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_refreeshTokens_StudentId",
                table: "refreeshTokens",
                column: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_refreeshTokens_Students_StudentId",
                table: "refreeshTokens",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

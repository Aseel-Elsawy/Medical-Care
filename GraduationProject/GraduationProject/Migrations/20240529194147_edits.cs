using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GraduationProject.Migrations
{
    /// <inheritdoc />
    public partial class edits : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_doctors_AspNetUsers_UserId",
                table: "doctors");

            migrationBuilder.DropForeignKey(
                name: "FK_patients_AspNetUsers_UserId",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_patients_UserId",
                table: "patients");

            migrationBuilder.DropIndex(
                name: "IX_doctors_UserId",
                table: "doctors");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "patients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "doctors");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "patients",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "doctors",
                type: "nvarchar(450)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_patients_UserId",
                table: "patients",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_doctors_UserId",
                table: "doctors",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_doctors_AspNetUsers_UserId",
                table: "doctors",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_patients_AspNetUsers_UserId",
                table: "patients",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University_system.Migrations
{
    /// <inheritdoc />
    public partial class init3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Semasters_SemasterId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SemasterId",
                table: "Courses",
                newName: "SemesterId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_SemasterId",
                table: "Courses",
                newName: "IX_Courses_SemesterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Semasters_SemesterId",
                table: "Courses",
                column: "SemesterId",
                principalTable: "Semasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Semasters_SemesterId",
                table: "Courses");

            migrationBuilder.RenameColumn(
                name: "SemesterId",
                table: "Courses",
                newName: "SemasterId");

            migrationBuilder.RenameIndex(
                name: "IX_Courses_SemesterId",
                table: "Courses",
                newName: "IX_Courses_SemasterId");

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Semasters_SemasterId",
                table: "Courses",
                column: "SemasterId",
                principalTable: "Semasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

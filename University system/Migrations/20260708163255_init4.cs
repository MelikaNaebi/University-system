using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace University_system.Migrations
{
    /// <inheritdoc />
    public partial class init4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Semasters_SemesterId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowRequests_Semasters_SemesterId",
                table: "WorkflowRequests");

            migrationBuilder.DropTable(
                name: "Semasters");

            migrationBuilder.CreateTable(
                name: "Semesters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semesters", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Semesters_SemesterId",
                table: "Courses",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowRequests_Semesters_SemesterId",
                table: "WorkflowRequests",
                column: "SemesterId",
                principalTable: "Semesters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Courses_Semesters_SemesterId",
                table: "Courses");

            migrationBuilder.DropForeignKey(
                name: "FK_WorkflowRequests_Semesters_SemesterId",
                table: "WorkflowRequests");

            migrationBuilder.DropTable(
                name: "Semesters");

            migrationBuilder.CreateTable(
                name: "Semasters",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    IsCurrent = table.Column<bool>(type: "bit", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Semasters", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_Courses_Semasters_SemesterId",
                table: "Courses",
                column: "SemesterId",
                principalTable: "Semasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_WorkflowRequests_Semasters_SemesterId",
                table: "WorkflowRequests",
                column: "SemesterId",
                principalTable: "Semasters",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

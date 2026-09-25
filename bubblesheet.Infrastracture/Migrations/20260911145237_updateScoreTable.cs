using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bubblesheet.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class updateScoreTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Score",
                table: "Students");

            migrationBuilder.CreateTable(
                name: "StudentScores",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Score = table.Column<int>(type: "int", nullable: false),
                    AcYearId = table.Column<int>(type: "int", nullable: false),
                    AcademicYearId = table.Column<int>(type: "int", nullable: false),
                    StudentId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentScores", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentScores_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "StudentId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StudentScores_academicYears_AcademicYearId",
                        column: x => x.AcademicYearId,
                        principalTable: "academicYears",
                        principalColumn: "AcademicYearId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_AcademicYearId",
                table: "StudentScores",
                column: "AcademicYearId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_StudentId",
                table: "StudentScores",
                column: "StudentId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentScores");

            migrationBuilder.AddColumn<int>(
                name: "Score",
                table: "Students",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }
    }
}

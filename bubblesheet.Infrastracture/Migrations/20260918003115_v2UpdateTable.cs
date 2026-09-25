using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bubblesheet.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class v2UpdateTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentScores_academicYears_AcademicYearId",
                table: "StudentScores");

            migrationBuilder.DropIndex(
                name: "IX_StudentScores_AcademicYearId",
                table: "StudentScores");

            migrationBuilder.DropColumn(
                name: "AcademicYearId",
                table: "StudentScores");

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_AcYearId",
                table: "StudentScores",
                column: "AcYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScores_academicYears_AcYearId",
                table: "StudentScores",
                column: "AcYearId",
                principalTable: "academicYears",
                principalColumn: "AcademicYearId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StudentScores_academicYears_AcYearId",
                table: "StudentScores");

            migrationBuilder.DropIndex(
                name: "IX_StudentScores_AcYearId",
                table: "StudentScores");

            migrationBuilder.AddColumn<int>(
                name: "AcademicYearId",
                table: "StudentScores",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StudentScores_AcademicYearId",
                table: "StudentScores",
                column: "AcademicYearId");

            migrationBuilder.AddForeignKey(
                name: "FK_StudentScores_academicYears_AcademicYearId",
                table: "StudentScores",
                column: "AcademicYearId",
                principalTable: "academicYears",
                principalColumn: "AcademicYearId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

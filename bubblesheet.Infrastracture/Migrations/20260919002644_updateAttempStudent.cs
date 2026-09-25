using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace bubblesheet.Infrastracture.Migrations
{
    /// <inheritdoc />
    public partial class updateAttempStudent : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsFirst",
                table: "studentAttempts",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFirst",
                table: "studentAttempts");
        }
    }
}

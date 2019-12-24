using Microsoft.EntityFrameworkCore.Migrations;

namespace TabSanat.Dal.Migrations
{
    public partial class CourseGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Groups",
                table: "Courses",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Groups",
                table: "Courses");
        }
    }
}

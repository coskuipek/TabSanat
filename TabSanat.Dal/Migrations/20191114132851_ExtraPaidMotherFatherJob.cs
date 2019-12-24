using Microsoft.EntityFrameworkCore.Migrations;

namespace TabSanat.Dal.Migrations
{
    public partial class ExtraPaidMotherFatherJob : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FatherJob",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "MotherJob",
                table: "Students",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "ExtraPaidLessonCount",
                table: "Registrations",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FatherJob",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "MotherJob",
                table: "Students");

            migrationBuilder.DropColumn(
                name: "ExtraPaidLessonCount",
                table: "Registrations");
        }
    }
}

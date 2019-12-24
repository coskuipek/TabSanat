using Microsoft.EntityFrameworkCore.Migrations;

namespace TabSanat.Dal.Migrations
{
    public partial class StudentSaleDelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Students_StudentId",
                table: "Sales");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Students_StudentId",
                table: "Sales",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Sales_Students_StudentId",
                table: "Sales");

            migrationBuilder.AddForeignKey(
                name: "FK_Sales_Students_StudentId",
                table: "Sales",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

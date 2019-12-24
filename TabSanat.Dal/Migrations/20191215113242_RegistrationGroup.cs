using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace TabSanat.Dal.Migrations
{
    public partial class RegistrationGroup : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "GroupId",
                table: "Registrations",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_GroupId",
                table: "Registrations",
                column: "GroupId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_Group_GroupId",
                table: "Registrations",
                column: "GroupId",
                principalTable: "Group",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_Group_GroupId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_GroupId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "GroupId",
                table: "Registrations");
        }
    }
}

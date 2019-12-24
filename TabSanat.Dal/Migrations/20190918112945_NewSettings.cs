using Microsoft.EntityFrameworkCore.Migrations;

namespace TabSanat.Dal.Migrations
{
    public partial class NewSettings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SettingName", "Value" },
                values: new object[] { "Firma Adı", "Tab Sanat" });

            migrationBuilder.InsertData(
                table: "AppSettings",
                columns: new[] { "Id", "SettingName", "Value" },
                values: new object[] { 2, "Ana Sayfada geç ödeme listesi için minimum sayı", "1" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.UpdateData(
                table: "AppSettings",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "SettingName", "Value" },
                values: new object[] { "Ana Sayfada geç ödeme listesi için minimum sayı", "1" });
        }
    }
}

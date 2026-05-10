using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class changedSeedUserInfo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Firstname", "Lastname" },
                values: new object[] { "دانا", "توانا" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Firstname", "Lastname" },
                values: new object[] { "کاربر", "پشتیبانی" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "Firstname", "Lastname" },
                values: new object[] { "admin", "admin" });

            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "Firstname", "Lastname" },
                values: new object[] { "support", "support" });
        }
    }
}

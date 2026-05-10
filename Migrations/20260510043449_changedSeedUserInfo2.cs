using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class changedSeedUserInfo2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Mobile",
                value: "09352222222");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "User",
                keyColumn: "Id",
                keyValue: 1,
                column: "Mobile",
                value: "09353947257");
        }
    }
}

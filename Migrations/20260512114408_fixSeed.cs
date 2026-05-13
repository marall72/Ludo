using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class fixSeed : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 17,
                column: "StationLevel",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 18,
                column: "StationLevel",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 19,
                column: "StationLevel",
                value: 3);

            migrationBuilder.UpdateData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 20,
                column: "StationLevel",
                value: 3);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 17,
                column: "StationLevel",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 18,
                column: "StationLevel",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 19,
                column: "StationLevel",
                value: 2);

            migrationBuilder.UpdateData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 20,
                column: "StationLevel",
                value: 2);
        }
    }
}

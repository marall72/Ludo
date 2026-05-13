using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class fixSeed2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Station",
                columns: new[] { "Id", "CreateDate", "CreatorId", "Description", "IsActive", "MapLeftPosition", "MapTopPosition", "PlayerCount", "StationLevel", "StationType", "Title", "UpdateDate", "UpdaterId" },
                values: new object[,]
                {
                    { 21, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 1, 0, "ARC1", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 22, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 1, 0, "ARC2", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 23, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 1, 0, "ARC3", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 23);
        }
    }
}

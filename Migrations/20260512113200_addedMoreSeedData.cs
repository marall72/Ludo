using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class addedMoreSeedData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Station",
                columns: new[] { "Id", "CreateDate", "CreatorId", "Description", "IsActive", "MapLeftPosition", "MapTopPosition", "PlayerCount", "StationLevel", "StationType", "Title", "UpdateDate", "UpdaterId" },
                values: new object[,]
                {
                    { 1, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R1", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 2, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R2", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 3, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R3", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 4, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R4", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 5, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R5", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 6, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R6", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 7, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R7", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 8, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R8", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 9, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R9", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 10, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 0, 0, "R10", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 11, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "U1", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 12, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "U2", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 13, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "U3", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 14, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "U4", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 15, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "U5", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 16, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "U6", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 17, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "PR7", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 18, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "PR8", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 19, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "PR9", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 },
                    { 20, new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1, null, true, 0m, 0m, 1, 2, 0, "PR10", new DateTime(2026, 5, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), 1 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 15);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "Station",
                keyColumn: "Id",
                keyValue: 20);
        }
    }
}

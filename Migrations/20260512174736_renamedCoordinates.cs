using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class renamedCoordinates : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "MapTopPosition",
                table: "Station",
                newName: "Y");

            migrationBuilder.RenameColumn(
                name: "MapLeftPosition",
                table: "Station",
                newName: "X");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Y",
                table: "Station",
                newName: "MapTopPosition");

            migrationBuilder.RenameColumn(
                name: "X",
                table: "Station",
                newName: "MapLeftPosition");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class addedMapPositionToStation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "MapLeftPosition",
                table: "Station",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<decimal>(
                name: "MapTopPosition",
                table: "Station",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MapLeftPosition",
                table: "Station");

            migrationBuilder.DropColumn(
                name: "MapTopPosition",
                table: "Station");
        }
    }
}

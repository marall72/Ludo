using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class increasedMapPrecision : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MapTopPosition",
                table: "Station",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MapLeftPosition",
                table: "Station",
                type: "decimal(18,10)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,2)");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<decimal>(
                name: "MapTopPosition",
                table: "Station",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)");

            migrationBuilder.AlterColumn<decimal>(
                name: "MapLeftPosition",
                table: "Station",
                type: "decimal(18,2)",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18,10)");
        }
    }
}

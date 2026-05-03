using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class fixingMessedupRelations : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationGame_Station_GameId",
                table: "StationGame");

            migrationBuilder.AddForeignKey(
                name: "FK_StationGame_Game_GameId",
                table: "StationGame",
                column: "GameId",
                principalTable: "Game",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StationGame_Game_GameId",
                table: "StationGame");

            migrationBuilder.AddForeignKey(
                name: "FK_StationGame_Station_GameId",
                table: "StationGame",
                column: "GameId",
                principalTable: "Station",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Ludo.Migrations
{
    /// <inheritdoc />
    public partial class seperatedStationFromReservation : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reservation_Station_StationId",
                table: "Reservation");

            migrationBuilder.DropIndex(
                name: "IX_Reservation_StationId",
                table: "Reservation");

            migrationBuilder.CreateTable(
                name: "ReservationStations",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ReservationId = table.Column<int>(type: "int", nullable: false),
                    StationId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReservationStations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReservationStations_Reservation_ReservationId",
                        column: x => x.ReservationId,
                        principalTable: "Reservation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReservationStations_Station_StationId",
                        column: x => x.StationId,
                        principalTable: "Station",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ReservationStations_ReservationId",
                table: "ReservationStations",
                column: "ReservationId");

            migrationBuilder.CreateIndex(
                name: "IX_ReservationStations_StationId",
                table: "ReservationStations",
                column: "StationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ReservationStations");

            migrationBuilder.CreateIndex(
                name: "IX_Reservation_StationId",
                table: "Reservation",
                column: "StationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reservation_Station_StationId",
                table: "Reservation",
                column: "StationId",
                principalTable: "Station",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

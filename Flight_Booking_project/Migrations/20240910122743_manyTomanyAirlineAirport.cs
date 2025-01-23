using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight_Booking_project.Migrations
{
    /// <inheritdoc />
    public partial class manyTomanyAirlineAirport : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirlineAirport",
                columns: table => new
                {
                    AirlinesAirlineId = table.Column<int>(type: "int", nullable: false),
                    AirportsAirportId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirlineAirport", x => new { x.AirlinesAirlineId, x.AirportsAirportId });
                    table.ForeignKey(
                        name: "FK_AirlineAirport_Airlines_AirlinesAirlineId",
                        column: x => x.AirlinesAirlineId,
                        principalTable: "Airlines",
                        principalColumn: "AirlineId",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_AirlineAirport_Airports_AirportsAirportId",
                        column: x => x.AirportsAirportId,
                        principalTable: "Airports",
                        principalColumn: "AirportId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirlineAirport_AirportsAirportId",
                table: "AirlineAirport",
                column: "AirportsAirportId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirlineAirport");
        }
    }
}

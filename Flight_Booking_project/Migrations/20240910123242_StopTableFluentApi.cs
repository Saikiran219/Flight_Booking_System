using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight_Booking_project.Migrations
{
    /// <inheritdoc />
    public partial class StopTableFluentApi : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AirportId",
                table: "Stops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "FlightId",
                table: "Stops",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_AirportId",
                table: "Stops",
                column: "AirportId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stops_FlightId",
                table: "Stops",
                column: "FlightId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Airports_AirportId",
                table: "Stops",
                column: "AirportId",
                principalTable: "Airports",
                principalColumn: "AirportId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stops_Flights_FlightId",
                table: "Stops",
                column: "FlightId",
                principalTable: "Flights",
                principalColumn: "FlightId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Airports_AirportId",
                table: "Stops");

            migrationBuilder.DropForeignKey(
                name: "FK_Stops_Flights_FlightId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_AirportId",
                table: "Stops");

            migrationBuilder.DropIndex(
                name: "IX_Stops_FlightId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "AirportId",
                table: "Stops");

            migrationBuilder.DropColumn(
                name: "FlightId",
                table: "Stops");
        }
    }
}

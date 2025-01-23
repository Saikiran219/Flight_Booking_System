using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight_Booking_project.Migrations
{
    /// <inheritdoc />
    public partial class onetoOneBetweenSeatandPassenger : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "SeatId",
                table: "Passengers",
                type: "int",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers",
                column: "SeatId",
                unique: true,
                filter: "[SeatId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Passengers_Seats_SeatId",
                table: "Passengers",
                column: "SeatId",
                principalTable: "Seats",
                principalColumn: "SeatId",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Passengers_Seats_SeatId",
                table: "Passengers");

            migrationBuilder.DropIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers");

            migrationBuilder.DropColumn(
                name: "SeatId",
                table: "Passengers");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight_Booking_project.Migrations
{
    /// <inheritdoc />
    public partial class makingNotnullSeatid : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers");

            migrationBuilder.AlterColumn<int>(
                name: "SeatId",
                table: "Passengers",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers",
                column: "SeatId",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers");

            migrationBuilder.AlterColumn<int>(
                name: "SeatId",
                table: "Passengers",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Passengers_SeatId",
                table: "Passengers",
                column: "SeatId",
                unique: true,
                filter: "[SeatId] IS NOT NULL");
        }
    }
}

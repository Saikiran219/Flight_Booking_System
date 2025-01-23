using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Flight_Booking_project.Migrations
{
    /// <inheritdoc />
    public partial class addedStatusIdcoloumninbookingtable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Bookings");
        }
    }
}

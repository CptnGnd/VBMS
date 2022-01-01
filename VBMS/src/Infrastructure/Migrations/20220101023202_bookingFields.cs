using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBMS.Infrastructure.Migrations
{
    public partial class bookingFields : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BookingCode",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "BookingType",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookingCode",
                table: "Bookings");

            migrationBuilder.DropColumn(
                name: "BookingType",
                table: "Bookings");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBMS.Infrastructure.Migrations
{
    public partial class diarynullbooking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diarys_Bookings_BookingId",
                table: "Diarys");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Diarys",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddForeignKey(
                name: "FK_Diarys_Bookings_BookingId",
                table: "Diarys",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diarys_Bookings_BookingId",
                table: "Diarys");

            migrationBuilder.AlterColumn<int>(
                name: "BookingId",
                table: "Diarys",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Diarys_Bookings_BookingId",
                table: "Diarys",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

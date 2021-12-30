using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBMS.Infrastructure.Migrations
{
    public partial class vehicleImage : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageDataURL",
                table: "Vehicles",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageDataURL",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageDataURL",
                table: "Drivers",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageDataURL",
                table: "Vehicles");

            migrationBuilder.DropColumn(
                name: "ImageDataURL",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "ImageDataURL",
                table: "Drivers");
        }
    }
}

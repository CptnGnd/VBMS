using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace VBMS.Infrastructure.Migrations
{
    public partial class diaryType2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Dairys_Bookings_BookingId",
                table: "Dairys");

            migrationBuilder.DropForeignKey(
                name: "FK_Dairys_DairyTypes_DairyTypeId",
                table: "Dairys");

            migrationBuilder.DropForeignKey(
                name: "FK_Dairys_Vehicles_VehicleId",
                table: "Dairys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DairyTypes",
                table: "DairyTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Dairys",
                table: "Dairys");

            migrationBuilder.DropIndex(
                name: "IX_Dairys_DairyTypeId",
                table: "Dairys");

            migrationBuilder.DropColumn(
                name: "DairyTypeId",
                table: "Dairys");

            migrationBuilder.RenameTable(
                name: "DairyTypes",
                newName: "DiaryTypes");

            migrationBuilder.RenameTable(
                name: "Dairys",
                newName: "Diarys");

            migrationBuilder.RenameIndex(
                name: "IX_Dairys_VehicleId",
                table: "Diarys",
                newName: "IX_Diarys_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Dairys_BookingId",
                table: "Diarys",
                newName: "IX_Diarys_BookingId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_DiaryTypes",
                table: "DiaryTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Diarys",
                table: "Diarys",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Diarys_DiaryTypeId",
                table: "Diarys",
                column: "DiaryTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Diarys_Bookings_BookingId",
                table: "Diarys",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Diarys_DiaryTypes_DiaryTypeId",
                table: "Diarys",
                column: "DiaryTypeId",
                principalTable: "DiaryTypes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Diarys_Vehicles_VehicleId",
                table: "Diarys",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Diarys_Bookings_BookingId",
                table: "Diarys");

            migrationBuilder.DropForeignKey(
                name: "FK_Diarys_DiaryTypes_DiaryTypeId",
                table: "Diarys");

            migrationBuilder.DropForeignKey(
                name: "FK_Diarys_Vehicles_VehicleId",
                table: "Diarys");

            migrationBuilder.DropPrimaryKey(
                name: "PK_DiaryTypes",
                table: "DiaryTypes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Diarys",
                table: "Diarys");

            migrationBuilder.DropIndex(
                name: "IX_Diarys_DiaryTypeId",
                table: "Diarys");

            migrationBuilder.RenameTable(
                name: "DiaryTypes",
                newName: "DairyTypes");

            migrationBuilder.RenameTable(
                name: "Diarys",
                newName: "Dairys");

            migrationBuilder.RenameIndex(
                name: "IX_Diarys_VehicleId",
                table: "Dairys",
                newName: "IX_Dairys_VehicleId");

            migrationBuilder.RenameIndex(
                name: "IX_Diarys_BookingId",
                table: "Dairys",
                newName: "IX_Dairys_BookingId");

            migrationBuilder.AddColumn<int>(
                name: "DairyTypeId",
                table: "Dairys",
                type: "int",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_DairyTypes",
                table: "DairyTypes",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Dairys",
                table: "Dairys",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_Dairys_DairyTypeId",
                table: "Dairys",
                column: "DairyTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Dairys_Bookings_BookingId",
                table: "Dairys",
                column: "BookingId",
                principalTable: "Bookings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Dairys_DairyTypes_DairyTypeId",
                table: "Dairys",
                column: "DairyTypeId",
                principalTable: "DairyTypes",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Dairys_Vehicles_VehicleId",
                table: "Dairys",
                column: "VehicleId",
                principalTable: "Vehicles",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

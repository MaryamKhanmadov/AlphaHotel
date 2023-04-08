using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Alpha_Hotel_Project.Migrations
{
    public partial class UpdateRoomTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsParking",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "IsWifi",
                table: "Rooms");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsParking",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsWifi",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}

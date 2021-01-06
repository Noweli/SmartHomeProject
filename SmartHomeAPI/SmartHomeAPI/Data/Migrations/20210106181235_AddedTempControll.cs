using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHomeAPI.Data.Migrations
{
    public partial class AddedTempControll : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "AutoHeatEnabled",
                table: "Rooms",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Interval",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MaxTemp",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "MinTemp",
                table: "Rooms",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AutoHeatEnabled",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "Interval",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MaxTemp",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "MinTemp",
                table: "Rooms");
        }
    }
}

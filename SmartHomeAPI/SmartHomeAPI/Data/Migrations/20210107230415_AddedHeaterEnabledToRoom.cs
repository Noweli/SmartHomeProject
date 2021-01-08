using Microsoft.EntityFrameworkCore.Migrations;

namespace SmartHomeAPI.Data.Migrations
{
    public partial class AddedHeaterEnabledToRoom : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "HeaterEnabled",
                table: "Rooms",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "HeaterEnabled",
                table: "Rooms");
        }
    }
}

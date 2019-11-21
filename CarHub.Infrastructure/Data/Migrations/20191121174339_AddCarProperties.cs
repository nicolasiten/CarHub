using Microsoft.EntityFrameworkCore.Migrations;

namespace CarHub.Infrastructure.Data.Migrations
{
    public partial class AddCarProperties : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Kilometers",
                table: "Car",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TransmissionType",
                table: "Car",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Kilometers",
                table: "Car");

            migrationBuilder.DropColumn(
                name: "TransmissionType",
                table: "Car");
        }
    }
}

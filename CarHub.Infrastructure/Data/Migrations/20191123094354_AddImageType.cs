using Microsoft.EntityFrameworkCore.Migrations;

namespace CarHub.Infrastructure.Data.Migrations
{
    public partial class AddImageType : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "Thumbnail",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ImageType",
                table: "Image",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Thumbnail");

            migrationBuilder.DropColumn(
                name: "ImageType",
                table: "Image");
        }
    }
}

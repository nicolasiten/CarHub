using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarHub.Infrastructure.Data.Migrations
{
    public partial class AddImageTables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Image",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarFk = table.Column<int>(nullable: true),
                    File = table.Column<byte[]>(type: "image", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Image", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Image_Car_CarFk",
                        column: x => x.CarFk,
                        principalTable: "Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Thumbnail",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CarFk = table.Column<int>(nullable: true),
                    File = table.Column<byte[]>(type: "image", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Thumbnail", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Thumbnail_Car_CarFk",
                        column: x => x.CarFk,
                        principalTable: "Car",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Image_CarFk",
                table: "Image",
                column: "CarFk");

            migrationBuilder.CreateIndex(
                name: "IX_Thumbnail_CarFk",
                table: "Thumbnail",
                column: "CarFk",
                unique: true,
                filter: "[CarFk] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Image");

            migrationBuilder.DropTable(
                name: "Thumbnail");
        }
    }
}

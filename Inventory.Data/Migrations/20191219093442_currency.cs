using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class currency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ExaminationCommitte",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ExaminationCommitte");
        }
    }
}

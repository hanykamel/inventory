using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class book : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Number",
                table: "Book",
                newName: "PageCount");

            migrationBuilder.AddColumn<int>(
                name: "BookNumber",
                table: "Book",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BookNumber",
                table: "Book");

            migrationBuilder.RenameColumn(
                name: "PageCount",
                table: "Book",
                newName: "Number");
        }
    }
}

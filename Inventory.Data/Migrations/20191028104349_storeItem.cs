using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class storeItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NoteCreatorId",
                table: "StoreItem");

            migrationBuilder.AddColumn<string>(
                name: "RobbingName",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RobbingPrice",
                table: "StoreItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RobbingName",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "RobbingPrice",
                table: "StoreItem");

            migrationBuilder.AddColumn<string>(
                name: "NoteCreatorId",
                table: "StoreItem",
                maxLength: 450,
                nullable: true);
        }
    }
}

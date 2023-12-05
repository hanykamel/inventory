using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class storeitemdelete : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "Notification");

            migrationBuilder.AddColumn<bool>(
                name: "UnderDelete",
                table: "StoreItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UnderDelete",
                table: "StoreItem");

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "Notification",
                nullable: false,
                defaultValue: 0);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class refundorderDirectorder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "DirectOrderNotes",
                table: "RefundOrder",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDirectOrder",
                table: "RefundOrder",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "DirectOrderNotes",
                table: "RefundOrder");

            migrationBuilder.DropColumn(
                name: "IsDirectOrder",
                table: "RefundOrder");
        }
    }
}

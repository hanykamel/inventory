using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class updatestore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreId",
                table: "ExecutionOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrder_StoreId",
                table: "ExecutionOrder",
                column: "StoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrder_Store_StoreId",
                table: "ExecutionOrder",
                column: "StoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrder_Store_StoreId",
                table: "ExecutionOrder");

            migrationBuilder.DropIndex(
                name: "IX_ExecutionOrder_StoreId",
                table: "ExecutionOrder");

            migrationBuilder.DropColumn(
                name: "StoreId",
                table: "ExecutionOrder");
        }
    }
}

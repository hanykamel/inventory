using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class refundorderstoreitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrderStoreItem_StoreItem_StoreItemId",
                table: "RefundOrderStoreItem");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrderStoreItem_StoreItem",
                table: "RefundOrderStoreItem",
                column: "StoreItemId",
                principalTable: "StoreItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrderStoreItem_StoreItem",
                table: "RefundOrderStoreItem");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrderStoreItem_StoreItem_StoreItemId",
                table: "RefundOrderStoreItem",
                column: "StoreItemId",
                principalTable: "StoreItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

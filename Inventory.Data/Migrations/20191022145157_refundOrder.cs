using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class refundOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "StoreItemStatusId",
                table: "RefundOrderStoreItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrderStoreItem_StoreItemStatusId",
                table: "RefundOrderStoreItem",
                column: "StoreItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrderStoreItem_StoreItemStatus",
                table: "RefundOrderStoreItem",
                column: "StoreItemStatusId",
                principalTable: "StoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrderStoreItem_StoreItemStatus",
                table: "RefundOrderStoreItem");

            migrationBuilder.DropIndex(
                name: "IX_RefundOrderStoreItem_StoreItemStatusId",
                table: "RefundOrderStoreItem");

            migrationBuilder.DropColumn(
                name: "StoreItemStatusId",
                table: "RefundOrderStoreItem");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class stocktakingstoreitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTakingStoreItem_StockTaking_StockTakingId",
                table: "StockTakingStoreItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTakingStoreItem_StockTaking",
                table: "StockTakingStoreItem",
                column: "StockTakingId",
                principalTable: "StockTaking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StockTakingStoreItem_StockTaking",
                table: "StockTakingStoreItem");

            migrationBuilder.AddForeignKey(
                name: "FK_StockTakingStoreItem_StockTaking_StockTakingId",
                table: "StockTakingStoreItem",
                column: "StockTakingId",
                principalTable: "StockTaking",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

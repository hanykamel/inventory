using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class historystoreItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_StoreItemCopy_HistoryId",
                table: "StoreItemCopy",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_StoreItemCopy_StoreItemStatusId",
                table: "StoreItemCopy",
                column: "StoreItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItemCopy_StoreItem",
                table: "StoreItemCopy",
                column: "HistoryId",
                principalTable: "StoreItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItemCopy_StoreItemStatus",
                table: "StoreItemCopy",
                column: "StoreItemStatusId",
                principalTable: "StoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreItemCopy_StoreItem",
                table: "StoreItemCopy");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreItemCopy_StoreItemStatus",
                table: "StoreItemCopy");

            migrationBuilder.DropIndex(
                name: "IX_StoreItemCopy_HistoryId",
                table: "StoreItemCopy");

            migrationBuilder.DropIndex(
                name: "IX_StoreItemCopy_StoreItemStatusId",
                table: "StoreItemCopy");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class robbingstoreitemstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RobbingStoreItemStatusId",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RobbingStoreItemStatusId",
                table: "RobbedStoreItem",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_RobbingStoreItemStatusId",
                table: "StoreItem",
                column: "RobbingStoreItemStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbedStoreItem_RobbingStoreItemStatusId",
                table: "RobbedStoreItem",
                column: "RobbingStoreItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RobbedStoreItem_RobbingStoreItemStatus_RobbingStoreItemStatusId",
                table: "RobbedStoreItem",
                column: "RobbingStoreItemStatusId",
                principalTable: "RobbingStoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItem_RobbingStoreItemStatus_RobbingStoreItemStatusId",
                table: "StoreItem",
                column: "RobbingStoreItemStatusId",
                principalTable: "RobbingStoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobbedStoreItem_RobbingStoreItemStatus_RobbingStoreItemStatusId",
                table: "RobbedStoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreItem_RobbingStoreItemStatus_RobbingStoreItemStatusId",
                table: "StoreItem");

            migrationBuilder.DropIndex(
                name: "IX_StoreItem_RobbingStoreItemStatusId",
                table: "StoreItem");

            migrationBuilder.DropIndex(
                name: "IX_RobbedStoreItem_RobbingStoreItemStatusId",
                table: "RobbedStoreItem");

            migrationBuilder.DropColumn(
                name: "RobbingStoreItemStatusId",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "RobbingStoreItemStatusId",
                table: "RobbedStoreItem");
        }
    }
}

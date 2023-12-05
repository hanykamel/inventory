using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class RemoveRobbingStoreItemStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobbedStoreItem_RobbingStoreItemStatus_RobbingStoreItemStatusId",
                table: "RobbedStoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RobbingOrderStoreItem_RobbingStoreItemStatus",
                table: "RobbingOrderStoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreItem_RobbingStoreItemStatus_RobbingStoreItemStatusId",
                table: "StoreItem");

            migrationBuilder.DropTable(
                name: "RobbingStoreItemStatus");

            migrationBuilder.DropIndex(
                name: "IX_StoreItem_RobbingStoreItemStatusId",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "RobbingName",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "RobbingPrice",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "RobbingStoreItemStatusId",
                table: "StoreItem");

            migrationBuilder.RenameColumn(
                name: "RobbingStoreItemStatusId",
                table: "RobbingOrderStoreItem",
                newName: "StoreItemStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_RobbingOrderStoreItem_RobbingStoreItemStatusId",
                table: "RobbingOrderStoreItem",
                newName: "IX_RobbingOrderStoreItem_StoreItemStatusId");

            migrationBuilder.RenameColumn(
                name: "RobbingStoreItemStatusId",
                table: "RobbedStoreItem",
                newName: "StoreItemStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_RobbedStoreItem_RobbingStoreItemStatusId",
                table: "RobbedStoreItem",
                newName: "IX_RobbedStoreItem_StoreItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RobbedStoreItem_StoreItemStatus_StoreItemStatusId",
                table: "RobbedStoreItem",
                column: "StoreItemStatusId",
                principalTable: "StoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RobbingOrderStoreItem_StoreItemStatus",
                table: "RobbingOrderStoreItem",
                column: "StoreItemStatusId",
                principalTable: "StoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobbedStoreItem_StoreItemStatus_StoreItemStatusId",
                table: "RobbedStoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_RobbingOrderStoreItem_StoreItemStatus",
                table: "RobbingOrderStoreItem");

            migrationBuilder.RenameColumn(
                name: "StoreItemStatusId",
                table: "RobbingOrderStoreItem",
                newName: "RobbingStoreItemStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_RobbingOrderStoreItem_StoreItemStatusId",
                table: "RobbingOrderStoreItem",
                newName: "IX_RobbingOrderStoreItem_RobbingStoreItemStatusId");

            migrationBuilder.RenameColumn(
                name: "StoreItemStatusId",
                table: "RobbedStoreItem",
                newName: "RobbingStoreItemStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_RobbedStoreItem_StoreItemStatusId",
                table: "RobbedStoreItem",
                newName: "IX_RobbedStoreItem_RobbingStoreItemStatusId");

            migrationBuilder.AddColumn<string>(
                name: "RobbingName",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "RobbingPrice",
                table: "StoreItem",
                type: "decimal(18, 0)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RobbingStoreItemStatusId",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RobbingStoreItemStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingStoreItemStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_RobbingStoreItemStatusId",
                table: "StoreItem",
                column: "RobbingStoreItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RobbedStoreItem_RobbingStoreItemStatus_RobbingStoreItemStatusId",
                table: "RobbedStoreItem",
                column: "RobbingStoreItemStatusId",
                principalTable: "RobbingStoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RobbingOrderStoreItem_RobbingStoreItemStatus",
                table: "RobbingOrderStoreItem",
                column: "RobbingStoreItemStatusId",
                principalTable: "RobbingStoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItem_RobbingStoreItemStatus_RobbingStoreItemStatusId",
                table: "StoreItem",
                column: "RobbingStoreItemStatusId",
                principalTable: "RobbingStoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class tostore : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Transformation_ToStoreId",
                table: "Transformation",
                column: "ToStoreId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrder_ToStoreId",
                table: "RobbingOrder",
                column: "ToStoreId");

            migrationBuilder.AddForeignKey(
                name: "FK_RobbingOrder_ToStore",
                table: "RobbingOrder",
                column: "ToStoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transformation_ToStore",
                table: "Transformation",
                column: "ToStoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobbingOrder_ToStore",
                table: "RobbingOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Transformation_ToStore",
                table: "Transformation");

            migrationBuilder.DropIndex(
                name: "IX_Transformation_ToStoreId",
                table: "Transformation");

            migrationBuilder.DropIndex(
                name: "IX_RobbingOrder_ToStoreId",
                table: "RobbingOrder");
        }
    }
}

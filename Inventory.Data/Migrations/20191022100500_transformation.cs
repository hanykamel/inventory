using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class transformation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "Transformation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Serial",
                table: "Transformation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Transformation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_TransformationStoreItem_StoreItemId",
                table: "TransformationStoreItem",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_TransformationStoreItem_TransformationId",
                table: "TransformationStoreItem",
                column: "TransformationId");

            migrationBuilder.AddForeignKey(
                name: "FK_TransformationStoreItem_StoreItem",
                table: "TransformationStoreItem",
                column: "StoreItemId",
                principalTable: "StoreItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TransformationStoreItem_Transformation",
                table: "TransformationStoreItem",
                column: "TransformationId",
                principalTable: "Transformation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TransformationStoreItem_StoreItem",
                table: "TransformationStoreItem");

            migrationBuilder.DropForeignKey(
                name: "FK_TransformationStoreItem_Transformation",
                table: "TransformationStoreItem");

            migrationBuilder.DropIndex(
                name: "IX_TransformationStoreItem_StoreItemId",
                table: "TransformationStoreItem");

            migrationBuilder.DropIndex(
                name: "IX_TransformationStoreItem_TransformationId",
                table: "TransformationStoreItem");

            migrationBuilder.DropColumn(
                name: "Notes",
                table: "Transformation");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "Transformation");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Transformation");
        }
    }
}

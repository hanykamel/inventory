using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class unit5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDirectOrder",
                table: "RobbingOrder");

            migrationBuilder.AlterColumn<decimal>(
                name: "RobbingPrice",
                table: "StoreItem",
                type: "decimal(18, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_UnitId",
                table: "StoreItem",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItem_Unit",
                table: "StoreItem",
                column: "UnitId",
                principalTable: "Unit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StoreItem_Unit",
                table: "StoreItem");

            migrationBuilder.DropIndex(
                name: "IX_StoreItem_UnitId",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "StoreItem");

            migrationBuilder.AlterColumn<decimal>(
                name: "RobbingPrice",
                table: "StoreItem",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)",
                oldNullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDirectOrder",
                table: "RobbingOrder",
                nullable: false,
                defaultValue: false);
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class Addrobbingstoreitemstatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdditionNumber",
                table: "Transformation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "ExaminationReport",
                table: "RobbingOrderStoreItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "RobbingStoreItemStatusId",
                table: "RobbingOrderStoreItem",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "RobbingOrderStoreItem",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "AdditionNumber",
                table: "RobbingOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "AdditionNumber",
                table: "Addition",
                nullable: false,
                defaultValue: 0);

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
                name: "IX_RobbingOrderStoreItem_RobbingStoreItemStatusId",
                table: "RobbingOrderStoreItem",
                column: "RobbingStoreItemStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RobbingOrderStoreItem_RobbingStoreItemStatus",
                table: "RobbingOrderStoreItem",
                column: "RobbingStoreItemStatusId",
                principalTable: "RobbingStoreItemStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobbingOrderStoreItem_RobbingStoreItemStatus",
                table: "RobbingOrderStoreItem");

            migrationBuilder.DropTable(
                name: "RobbingStoreItemStatus");

            migrationBuilder.DropIndex(
                name: "IX_RobbingOrderStoreItem_RobbingStoreItemStatusId",
                table: "RobbingOrderStoreItem");

            migrationBuilder.DropColumn(
                name: "AdditionNumber",
                table: "Transformation");

            migrationBuilder.DropColumn(
                name: "ExaminationReport",
                table: "RobbingOrderStoreItem");

            migrationBuilder.DropColumn(
                name: "RobbingStoreItemStatusId",
                table: "RobbingOrderStoreItem");

            migrationBuilder.DropColumn(
                name: "price",
                table: "RobbingOrderStoreItem");

            migrationBuilder.DropColumn(
                name: "AdditionNumber",
                table: "RobbingOrder");

            migrationBuilder.DropColumn(
                name: "AdditionNumber",
                table: "Addition");
        }
    }
}

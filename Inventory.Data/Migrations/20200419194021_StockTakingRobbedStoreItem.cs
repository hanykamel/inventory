using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class StockTakingRobbedStoreItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockTakingRobbedStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    StockTakingId = table.Column<Guid>(nullable: false),
                    RobbedStoreItemId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakingRobbedStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTakingRobbedStoreItem_RobbedStoreItem_RobbedStoreItemId",
                        column: x => x.RobbedStoreItemId,
                        principalTable: "RobbedStoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTakingRobbedStoreItem_StockTaking",
                        column: x => x.StockTakingId,
                        principalTable: "StockTaking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTakingRobbedStoreItem_RobbedStoreItemId",
                table: "StockTakingRobbedStoreItem",
                column: "RobbedStoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakingRobbedStoreItem_StockTakingId",
                table: "StockTakingRobbedStoreItem",
                column: "StockTakingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTakingRobbedStoreItem");
        }
    }
}

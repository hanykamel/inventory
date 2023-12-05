using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class relationExecution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionOrderStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    ExecutionOrderId = table.Column<Guid>(nullable: false),
                    IsApproved = table.Column<bool>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionOrderStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderStoreItem_ExecutionOrder_ExecutionOrderId",
                        column: x => x.ExecutionOrderId,
                        principalTable: "ExecutionOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderStoreItem_StoreItem_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderStoreItem_ExecutionOrderId",
                table: "ExecutionOrderStoreItem",
                column: "ExecutionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderStoreItem_StoreItemId",
                table: "ExecutionOrderStoreItem",
                column: "StoreItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionOrderStoreItem");
        }
    }
}

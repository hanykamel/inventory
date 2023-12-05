using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class createtables : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionOrderResultItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    BaseItemId = table.Column<long>(nullable: false),
                    ExecutionOrderId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionOrderResultItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderResultItem_BaseItem_BaseItemId",
                        column: x => x.BaseItemId,
                        principalTable: "BaseItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderResultItem_ExecutionOrder_ExecutionOrderId",
                        column: x => x.ExecutionOrderId,
                        principalTable: "ExecutionOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderResultItem_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionOrderResultRemain",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    RemainsId = table.Column<Guid>(nullable: false),
                    ExecutionOrderId = table.Column<Guid>(nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    RemainsId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionOrderResultRemain", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderResultRemain_ExecutionOrder_ExecutionOrderId",
                        column: x => x.ExecutionOrderId,
                        principalTable: "ExecutionOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderResultRemain_Remains_RemainsId1",
                        column: x => x.RemainsId1,
                        principalTable: "Remains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderResultRemain_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultItem_BaseItemId",
                table: "ExecutionOrderResultItem",
                column: "BaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultItem_ExecutionOrderId",
                table: "ExecutionOrderResultItem",
                column: "ExecutionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultItem_UnitId",
                table: "ExecutionOrderResultItem",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultRemain_ExecutionOrderId",
                table: "ExecutionOrderResultRemain",
                column: "ExecutionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultRemain_RemainsId1",
                table: "ExecutionOrderResultRemain",
                column: "RemainsId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultRemain_UnitId",
                table: "ExecutionOrderResultRemain",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionOrderResultItem");

            migrationBuilder.DropTable(
                name: "ExecutionOrderResultRemain");
        }
    }
}

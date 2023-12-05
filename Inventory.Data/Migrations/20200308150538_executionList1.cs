using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class executionList1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultItem_BaseItem_BaseItemId",
                table: "ExecutionOrderResultItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultItem_ExecutionOrder_ExecutionOrderId",
                table: "ExecutionOrderResultItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultRemain_ExecutionOrder_ExecutionOrderId",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultRemain_Remains_RemainsId",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.CreateTable(
                name: "RobbingOrderRemainsDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    RobbingOrderId = table.Column<Guid>(nullable: false),
                    ExecutionOrderResultRemainId = table.Column<Guid>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    price = table.Column<decimal>(nullable: false),
                    ExaminationReport = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingOrderRemainsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobbingOrderRemainsDetails_ExecutionOrderResultRemain_ExecutionOrderResultRemainId",
                        column: x => x.ExecutionOrderResultRemainId,
                        principalTable: "ExecutionOrderResultRemain",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RobbingOrderRemainsDetails_RobbingOrder_RobbingOrderId",
                        column: x => x.RobbingOrderId,
                        principalTable: "RobbingOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrderRemainsDetails_ExecutionOrderResultRemainId",
                table: "RobbingOrderRemainsDetails",
                column: "ExecutionOrderResultRemainId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrderRemainsDetails_RobbingOrderId",
                table: "RobbingOrderRemainsDetails",
                column: "RobbingOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultItem_BaseItem",
                table: "ExecutionOrderResultItem",
                column: "BaseItemId",
                principalTable: "BaseItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultItem_ExecutionOrder",
                table: "ExecutionOrderResultItem",
                column: "ExecutionOrderId",
                principalTable: "ExecutionOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultRemain_ExecutionOrder",
                table: "ExecutionOrderResultRemain",
                column: "ExecutionOrderId",
                principalTable: "ExecutionOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultRemain_Remains",
                table: "ExecutionOrderResultRemain",
                column: "RemainsId",
                principalTable: "Remains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultItem_BaseItem",
                table: "ExecutionOrderResultItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultItem_ExecutionOrder",
                table: "ExecutionOrderResultItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultRemain_ExecutionOrder",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultRemain_Remains",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropTable(
                name: "RobbingOrderRemainsDetails");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultItem_BaseItem_BaseItemId",
                table: "ExecutionOrderResultItem",
                column: "BaseItemId",
                principalTable: "BaseItem",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultItem_ExecutionOrder_ExecutionOrderId",
                table: "ExecutionOrderResultItem",
                column: "ExecutionOrderId",
                principalTable: "ExecutionOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultRemain_ExecutionOrder_ExecutionOrderId",
                table: "ExecutionOrderResultRemain",
                column: "ExecutionOrderId",
                principalTable: "ExecutionOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultRemain_Remains_RemainsId",
                table: "ExecutionOrderResultRemain",
                column: "RemainsId",
                principalTable: "Remains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

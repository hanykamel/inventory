using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class modificationExecution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExecutionOrderId",
                table: "RemainsDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RemainsDetails_ExecutionOrderId",
                table: "RemainsDetails",
                column: "ExecutionOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_RemainsDetails_ExecutionOrder_ExecutionOrderId",
                table: "RemainsDetails",
                column: "ExecutionOrderId",
                principalTable: "ExecutionOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RemainsDetails_ExecutionOrder_ExecutionOrderId",
                table: "RemainsDetails");

            migrationBuilder.DropIndex(
                name: "IX_RemainsDetails_ExecutionOrderId",
                table: "RemainsDetails");

            migrationBuilder.DropColumn(
                name: "ExecutionOrderId",
                table: "RemainsDetails");
        }
    }
}

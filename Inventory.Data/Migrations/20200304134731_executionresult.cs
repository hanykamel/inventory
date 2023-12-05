using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class executionresult : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AddColumn<bool>(
                name: "UnderExecution",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "BookId",
                table: "RemainsDetails",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<int>(
                name: "BookPageNumber",
                table: "RemainsDetails",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Remains",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ReviewNotes",
                table: "ExecutionOrder",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ExecutionOrderId",
                table: "Addition",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RemainsDetails_BookId",
                table: "RemainsDetails",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_Addition_ExecutionOrderId",
                table: "Addition",
                column: "ExecutionOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addition_ExecutionOrder_ExecutionOrderId",
                table: "Addition",
                column: "ExecutionOrderId",
                principalTable: "ExecutionOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RemainsDetails_Book_BookId",
                table: "RemainsDetails",
                column: "BookId",
                principalTable: "Book",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addition_ExecutionOrder_ExecutionOrderId",
                table: "Addition");

            migrationBuilder.DropForeignKey(
                name: "FK_RemainsDetails_Book_BookId",
                table: "RemainsDetails");

            migrationBuilder.DropIndex(
                name: "IX_RemainsDetails_BookId",
                table: "RemainsDetails");

            migrationBuilder.DropIndex(
                name: "IX_Addition_ExecutionOrderId",
                table: "Addition");

            migrationBuilder.DropColumn(
                name: "UnderExecution",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "BookId",
                table: "RemainsDetails");

            migrationBuilder.DropColumn(
                name: "BookPageNumber",
                table: "RemainsDetails");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Remains");

            migrationBuilder.DropColumn(
                name: "ReviewNotes",
                table: "ExecutionOrder");

            migrationBuilder.DropColumn(
                name: "ExecutionOrderId",
                table: "Addition");

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
    }
}

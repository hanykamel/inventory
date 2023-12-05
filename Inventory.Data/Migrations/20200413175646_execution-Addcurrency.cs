using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class executionAddcurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Date",
                table: "RobbingOrder");

            migrationBuilder.AddColumn<Guid>(
                name: "DeductionId",
                table: "Subtraction",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "RemainsDetails",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "ExecutionOrderResultRemain",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "ExecutionOrderResultItem",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Subtraction_DeductionId",
                table: "Subtraction",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultRemain_CurrencyId",
                table: "ExecutionOrderResultRemain",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultItem_CurrencyId",
                table: "ExecutionOrderResultItem",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultItem_Currency_CurrencyId",
                table: "ExecutionOrderResultItem",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultRemain_Currency_CurrencyId",
                table: "ExecutionOrderResultRemain",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Subtraction_Deduction",
                table: "Subtraction",
                column: "DeductionId",
                principalTable: "Deduction",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultItem_Currency_CurrencyId",
                table: "ExecutionOrderResultItem");

            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultRemain_Currency_CurrencyId",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropForeignKey(
                name: "FK_Subtraction_Deduction",
                table: "Subtraction");

            migrationBuilder.DropIndex(
                name: "IX_Subtraction_DeductionId",
                table: "Subtraction");

            migrationBuilder.DropIndex(
                name: "IX_ExecutionOrderResultRemain_CurrencyId",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropIndex(
                name: "IX_ExecutionOrderResultItem_CurrencyId",
                table: "ExecutionOrderResultItem");

            migrationBuilder.DropColumn(
                name: "DeductionId",
                table: "Subtraction");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "RemainsDetails");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ExecutionOrderResultItem");

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "RobbingOrder",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }
    }
}

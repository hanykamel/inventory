using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class invoice : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExchangeOrderId",
                table: "Invoice",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_ExchangeOrderId",
                table: "Invoice",
                column: "ExchangeOrderId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_ExchangeOrder",
                table: "Invoice",
                column: "ExchangeOrderId",
                principalTable: "ExchangeOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_ExchangeOrder",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_ExchangeOrderId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "ExchangeOrderId",
                table: "Invoice");
        }
    }
}

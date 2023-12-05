using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class invoiceStoreItem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsRefunded",
                table: "InvoiceStoreItem",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "RefundDate",
                table: "InvoiceStoreItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "RefundUserName",
                table: "InvoiceStoreItem",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsRefunded",
                table: "InvoiceStoreItem");

            migrationBuilder.DropColumn(
                name: "RefundDate",
                table: "InvoiceStoreItem");

            migrationBuilder.DropColumn(
                name: "RefundUserName",
                table: "InvoiceStoreItem");
        }
    }
}

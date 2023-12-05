using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class invoice_update : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "LocationId",
                table: "Invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Serial",
                table: "Invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "Invoice",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Serial",
                table: "ExchangeOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "TenantId",
                table: "ExchangeOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "Year",
                table: "ExchangeOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_LocationId",
                table: "Invoice",
                column: "LocationId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Locations",
                table: "Invoice",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Locations",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_LocationId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "LocationId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "Serial",
                table: "ExchangeOrder");

            migrationBuilder.DropColumn(
                name: "TenantId",
                table: "ExchangeOrder");

            migrationBuilder.DropColumn(
                name: "Year",
                table: "ExchangeOrder");
        }
    }
}

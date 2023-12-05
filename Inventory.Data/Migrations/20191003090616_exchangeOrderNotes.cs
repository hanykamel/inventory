using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class exchangeOrderNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "BugetId",
                table: "ExchangeOrder",
                newName: "BudgetId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeOrder_BugetId",
                table: "ExchangeOrder",
                newName: "IX_ExchangeOrder_BudgetId");

            migrationBuilder.AddColumn<string>(
                name: "Notes",
                table: "ExchangeOrder",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Notes",
                table: "ExchangeOrder");

            migrationBuilder.RenameColumn(
                name: "BudgetId",
                table: "ExchangeOrder",
                newName: "BugetId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeOrder_BudgetId",
                table: "ExchangeOrder",
                newName: "IX_ExchangeOrder_BugetId");
        }
    }
}

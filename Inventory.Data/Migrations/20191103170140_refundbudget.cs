using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class refundbudget : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrder_Budget",
                table: "RefundOrder");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetId",
                table: "RefundOrder",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AddColumn<int>(
                name: "RefundOrderEmployeeId",
                table: "RefundOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrder_Budget_BudgetId",
                table: "RefundOrder",
                column: "BudgetId",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrder_Budget_BudgetId",
                table: "RefundOrder");

            migrationBuilder.DropColumn(
                name: "RefundOrderEmployeeId",
                table: "RefundOrder");

            migrationBuilder.AlterColumn<int>(
                name: "BudgetId",
                table: "RefundOrder",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrder_Budget",
                table: "RefundOrder",
                column: "BudgetId",
                principalTable: "Budget",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

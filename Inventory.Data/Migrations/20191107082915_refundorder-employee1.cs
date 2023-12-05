using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class refundorderemployee1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrder_Employees_ExaminationEmployeeId",
                table: "RefundOrder");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrder_RefundOrderEmployeeId",
                table: "RefundOrder",
                column: "RefundOrderEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrder_examinationemployeeRefundOrder",
                table: "RefundOrder",
                column: "ExaminationEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrder_RefundOrderEmployee",
                table: "RefundOrder",
                column: "RefundOrderEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrder_examinationemployeeRefundOrder",
                table: "RefundOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrder_RefundOrderEmployee",
                table: "RefundOrder");

            migrationBuilder.DropIndex(
                name: "IX_RefundOrder_RefundOrderEmployeeId",
                table: "RefundOrder");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrder_Employees_ExaminationEmployeeId",
                table: "RefundOrder",
                column: "ExaminationEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

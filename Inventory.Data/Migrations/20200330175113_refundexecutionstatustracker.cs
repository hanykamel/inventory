using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class refundexecutionstatustracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionOrderStatusTracker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ExecutionOrderStatusId = table.Column<int>(nullable: false),
                    ExecutionOrderId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionOrderStatusTracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderStatusTracker_ExecutionOrder",
                        column: x => x.ExecutionOrderId,
                        principalTable: "ExecutionOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderStatusTracker_ExecutionOrderStatus",
                        column: x => x.ExecutionOrderStatusId,
                        principalTable: "ExecutionOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefundOrderStatusTracker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    RefundOrderStatusId = table.Column<int>(nullable: false),
                    RefundOrderId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundOrderStatusTracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundOrderStatusTracker_RefundOrder",
                        column: x => x.RefundOrderId,
                        principalTable: "RefundOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundOrderStatusTracker_RefundOrderStatus",
                        column: x => x.RefundOrderStatusId,
                        principalTable: "RefundOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderStatusTracker_ExecutionOrderId",
                table: "ExecutionOrderStatusTracker",
                column: "ExecutionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderStatusTracker_ExecutionOrderStatusId",
                table: "ExecutionOrderStatusTracker",
                column: "ExecutionOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrderStatusTracker_RefundOrderId",
                table: "RefundOrderStatusTracker",
                column: "RefundOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrderStatusTracker_RefundOrderStatusId",
                table: "RefundOrderStatusTracker",
                column: "RefundOrderStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionOrderStatusTracker");

            migrationBuilder.DropTable(
                name: "RefundOrderStatusTracker");
        }
    }
}

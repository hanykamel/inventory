using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class execution : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExecutionOrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    ExecutionOrderStatusId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    Year = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionOrder_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionOrder_ExecutionOrderStatus_ExecutionOrderStatusId",
                        column: x => x.ExecutionOrderStatusId,
                        principalTable: "ExecutionOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ExecutionOrder_Operation_OperationId",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ExecutionOrderAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ExecutionOrderId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExecutionOrderAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExecutionOrderAttachment_ExecutionOrder",
                        column: x => x.ExecutionOrderId,
                        principalTable: "ExecutionOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrder_BudgetId",
                table: "ExecutionOrder",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrder_ExecutionOrderStatusId",
                table: "ExecutionOrder",
                column: "ExecutionOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrder_OperationId",
                table: "ExecutionOrder",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderAttachment_AttachmentId",
                table: "ExecutionOrderAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderAttachment_ExecutionOrderId",
                table: "ExecutionOrderAttachment",
                column: "ExecutionOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExecutionOrderAttachment");

            migrationBuilder.DropTable(
                name: "ExecutionOrder");

            migrationBuilder.DropTable(
                name: "ExecutionOrderStatus");
        }
    }
}

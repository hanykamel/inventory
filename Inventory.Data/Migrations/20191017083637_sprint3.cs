using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class sprint3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RobbingOrderId",
                table: "Invoice",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RobbingOrderId",
                table: "Addition",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "RefundOrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RobbingOrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RefundOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    ExaminationEmployeeId = table.Column<int>(nullable: false),
                    RefundOrderStatusId = table.Column<int>(nullable: true),
                    Notes = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundOrder_Budget",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefundOrder_Employees_ExaminationEmployeeId",
                        column: x => x.ExaminationEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefundOrder_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundOrder_RefundOrderStatus",
                        column: x => x.RefundOrderStatusId,
                        principalTable: "RefundOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RobbingOrder",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    ForEmployeeId = table.Column<int>(nullable: false),
                    DirectOrderNotes = table.Column<string>(nullable: true),
                    RobbingOrderStatusId = table.Column<int>(nullable: true),
                    IsDirectOrder = table.Column<bool>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingOrder", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobbingOrder_Budget",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RobbingOrder_Employees_ForEmployeeId",
                        column: x => x.ForEmployeeId,
                        principalTable: "Employees",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RobbingOrder_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingOrder_RobbingOrderStatus",
                        column: x => x.RobbingOrderStatusId,
                        principalTable: "RobbingOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefundOrderAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    RefundOrderId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundOrderAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundOrderAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RefundOrderAttachment_RefundOrder",
                        column: x => x.RefundOrderId,
                        principalTable: "RefundOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RefundOrderStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    RefundOrderId = table.Column<Guid>(nullable: false),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundOrderStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RefundOrderStoreItem_RefundOrder_RefundOrderId",
                        column: x => x.RefundOrderId,
                        principalTable: "RefundOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RefundOrderStoreItem_StoreItem_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "RobbingOrderAttachment",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    RobbingOrderId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingOrderAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobbingOrderAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingOrderAttachment_RobbingOrder",
                        column: x => x.RobbingOrderId,
                        principalTable: "RobbingOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "RobbingOrderStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    RobbingOrderId = table.Column<Guid>(nullable: false),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    Notes = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingOrderStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobbingOrderStoreItem_RobbingOrder_RobbingOrderId",
                        column: x => x.RobbingOrderId,
                        principalTable: "RobbingOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RobbingOrderStoreItem_StoreItem_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_RobbingOrderId",
                table: "Invoice",
                column: "RobbingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Addition_RobbingOrderId",
                table: "Addition",
                column: "RobbingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrder_BudgetId",
                table: "RefundOrder",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrder_ExaminationEmployeeId",
                table: "RefundOrder",
                column: "ExaminationEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrder_OperationId",
                table: "RefundOrder",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrder_RefundOrderStatusId",
                table: "RefundOrder",
                column: "RefundOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrderAttachment_AttachmentId",
                table: "RefundOrderAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrderAttachment_RefundOrderId",
                table: "RefundOrderAttachment",
                column: "RefundOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrderStoreItem_RefundOrderId",
                table: "RefundOrderStoreItem",
                column: "RefundOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrderStoreItem_StoreItemId",
                table: "RefundOrderStoreItem",
                column: "StoreItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrder_BudgetId",
                table: "RobbingOrder",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrder_ForEmployeeId",
                table: "RobbingOrder",
                column: "ForEmployeeId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrder_OperationId",
                table: "RobbingOrder",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrder_RobbingOrderStatusId",
                table: "RobbingOrder",
                column: "RobbingOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrderAttachment_AttachmentId",
                table: "RobbingOrderAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrderAttachment_RobbingOrderId",
                table: "RobbingOrderAttachment",
                column: "RobbingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrderStoreItem_RobbingOrderId",
                table: "RobbingOrderStoreItem",
                column: "RobbingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrderStoreItem_StoreItemId",
                table: "RobbingOrderStoreItem",
                column: "StoreItemId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addition_RobbingOrder_RobbingOrderId",
                table: "Addition",
                column: "RobbingOrderId",
                principalTable: "RobbingOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_RobbingOrder_RobbingOrderId",
                table: "Invoice",
                column: "RobbingOrderId",
                principalTable: "RobbingOrder",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addition_RobbingOrder_RobbingOrderId",
                table: "Addition");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_RobbingOrder_RobbingOrderId",
                table: "Invoice");

            migrationBuilder.DropTable(
                name: "RefundOrderAttachment");

            migrationBuilder.DropTable(
                name: "RefundOrderStoreItem");

            migrationBuilder.DropTable(
                name: "RobbingOrderAttachment");

            migrationBuilder.DropTable(
                name: "RobbingOrderStoreItem");

            migrationBuilder.DropTable(
                name: "RefundOrder");

            migrationBuilder.DropTable(
                name: "RobbingOrder");

            migrationBuilder.DropTable(
                name: "RefundOrderStatus");

            migrationBuilder.DropTable(
                name: "RobbingOrderStatus");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_RobbingOrderId",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Addition_RobbingOrderId",
                table: "Addition");

            migrationBuilder.DropColumn(
                name: "RobbingOrderId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "RobbingOrderId",
                table: "Addition");
        }
    }
}

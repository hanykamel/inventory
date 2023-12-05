using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class deduction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Deduction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    Code = table.Column<string>(nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    BudgetId = table.Column<int>(nullable: false),
                    OperationId = table.Column<int>(nullable: false),
                    Notes = table.Column<string>(nullable: true),
                    RequesterName = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Deduction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Deduction_Budget_BudgetId",
                        column: x => x.BudgetId,
                        principalTable: "Budget",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Deduction_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeductionAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    DeductionId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeductionAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DeductionAttachment_Deduction",
                        column: x => x.DeductionId,
                        principalTable: "Deduction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "DeductionStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    DeductionId = table.Column<Guid>(nullable: false),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DeductionStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DeductionStoreItem_Deduction_DeductionId",
                        column: x => x.DeductionId,
                        principalTable: "Deduction",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_DeductionStoreItem_StoreItem_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_BudgetId",
                table: "Deduction",
                column: "BudgetId");

            migrationBuilder.CreateIndex(
                name: "IX_Deduction_OperationId",
                table: "Deduction",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_DeductionAttachment_AttachmentId",
                table: "DeductionAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_DeductionAttachment_DeductionId",
                table: "DeductionAttachment",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_DeductionStoreItem_DeductionId",
                table: "DeductionStoreItem",
                column: "DeductionId");

            migrationBuilder.CreateIndex(
                name: "IX_DeductionStoreItem_StoreItemId",
                table: "DeductionStoreItem",
                column: "StoreItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DeductionAttachment");

            migrationBuilder.DropTable(
                name: "DeductionStoreItem");

            migrationBuilder.DropTable(
                name: "Deduction");
        }
    }
}

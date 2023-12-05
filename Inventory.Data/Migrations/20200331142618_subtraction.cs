using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class subtraction : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "price",
                table: "RobbingOrderStoreItem",
                newName: "Price");

            migrationBuilder.RenameColumn(
                name: "price",
                table: "RobbingOrderRemainsDetails",
                newName: "Price");

            migrationBuilder.AlterColumn<decimal>(
                name: "RobbingPrice",
                table: "StoreItemCopy",
                type: "decimal(18, 0)",
                nullable: true,
                oldClrType: typeof(decimal),
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "StoreItemCopy",
                type: "decimal(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "RobbingOrderStoreItem",
                type: "decimal(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "RobbingOrderRemainsDetails",
                type: "decimal(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "RemainsDetails",
                type: "decimal(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "RemainsDetails",
                type: "decimal(18, 0)",
                nullable: false,
                oldClrType: typeof(decimal));

            migrationBuilder.CreateTable(
                name: "Subtraction",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    TransformationId = table.Column<Guid>(nullable: true),
                    RobbingOrderId = table.Column<Guid>(nullable: true),
                    ExecutionOrderId = table.Column<Guid>(nullable: true),
                    OperationId = table.Column<int>(nullable: false),
                    RequesterName = table.Column<string>(nullable: true),
                    Date = table.Column<DateTime>(nullable: false),
                    RequestDate = table.Column<DateTime>(nullable: false),
                    SubtractionNumber = table.Column<int>(nullable: true),
                    TenantId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Subtraction", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Subtraction_ExecutionOrder",
                        column: x => x.ExecutionOrderId,
                        principalTable: "ExecutionOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subtraction_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subtraction_RobbingOrder",
                        column: x => x.RobbingOrderId,
                        principalTable: "RobbingOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Subtraction_Transformation",
                        column: x => x.TransformationId,
                        principalTable: "Transformation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Subtraction_ExecutionOrderId",
                table: "Subtraction",
                column: "ExecutionOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtraction_OperationId",
                table: "Subtraction",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtraction_RobbingOrderId",
                table: "Subtraction",
                column: "RobbingOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_Subtraction_TransformationId",
                table: "Subtraction",
                column: "TransformationId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Subtraction");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "RobbingOrderStoreItem",
                newName: "price");

            migrationBuilder.RenameColumn(
                name: "Price",
                table: "RobbingOrderRemainsDetails",
                newName: "price");

            migrationBuilder.AlterColumn<decimal>(
                name: "RobbingPrice",
                table: "StoreItemCopy",
                nullable: true,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)",
                oldNullable: true);

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "StoreItemCopy",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "RobbingOrderStoreItem",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "price",
                table: "RobbingOrderRemainsDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Quantity",
                table: "RemainsDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)");

            migrationBuilder.AlterColumn<decimal>(
                name: "Price",
                table: "RemainsDetails",
                nullable: false,
                oldClrType: typeof(decimal),
                oldType: "decimal(18, 0)");
        }
    }
}

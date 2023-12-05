using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class stocktaking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockTaking",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    OperationId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Year = table.Column<int>(nullable: false),
                    Serial = table.Column<int>(nullable: false),
                    Code = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTaking", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTacking_Operation",
                        column: x => x.OperationId,
                        principalTable: "Operation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StockTakingStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    StockTakingId = table.Column<Guid>(nullable: false),
                    StoreItemId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakingStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTakingStoreItem_StockTaking_StockTakingId",
                        column: x => x.StockTakingId,
                        principalTable: "StockTaking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_StockTakingStoreItem_StoreItem_StoreItemId",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTaking_OperationId",
                table: "StockTaking",
                column: "OperationId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakingStoreItem_StockTakingId",
                table: "StockTakingStoreItem",
                column: "StockTakingId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakingStoreItem_StoreItemId",
                table: "StockTakingStoreItem",
                column: "StoreItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTakingStoreItem");

            migrationBuilder.DropTable(
                name: "StockTaking");
        }
    }
}

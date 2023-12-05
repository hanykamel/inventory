using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class robbedstoreitem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RobbingStoreItem");

            migrationBuilder.CreateTable(
                name: "RobbedStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    AdditionId = table.Column<Guid>(nullable: false),
                    StoreBaseItemId = table.Column<long>(nullable: false),
                    BaseItemId = table.Column<long>(nullable: false),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    BookId = table.Column<long>(nullable: false),
                    BookPageNumber = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    UnitId = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbedStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_Addition",
                        column: x => x.AdditionId,
                        principalTable: "Addition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_BaseItem",
                        column: x => x.BaseItemId,
                        principalTable: "BaseItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_Book",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_Currency",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbedStoreItem_BaseItem_StoreBaseItemId",
                        column: x => x.StoreBaseItemId,
                        principalTable: "BaseItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_Unit",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemainsDetails_CurrencyId",
                table: "RemainsDetails",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbedStoreItem_AdditionId",
                table: "RobbedStoreItem",
                column: "AdditionId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbedStoreItem_BaseItemId",
                table: "RobbedStoreItem",
                column: "BaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbedStoreItem_BookId",
                table: "RobbedStoreItem",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbedStoreItem_CurrencyId",
                table: "RobbedStoreItem",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbedStoreItem_StoreBaseItemId",
                table: "RobbedStoreItem",
                column: "StoreBaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbedStoreItem_UnitId",
                table: "RobbedStoreItem",
                column: "UnitId");

            migrationBuilder.AddForeignKey(
                name: "FK_RemainsDetails_Currency_CurrencyId",
                table: "RemainsDetails",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RemainsDetails_Currency_CurrencyId",
                table: "RemainsDetails");

            migrationBuilder.DropTable(
                name: "RobbedStoreItem");

            migrationBuilder.DropIndex(
                name: "IX_RemainsDetails_CurrencyId",
                table: "RemainsDetails");

            migrationBuilder.CreateTable(
                name: "RobbingStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    AdditionId = table.Column<Guid>(nullable: false),
                    BaseItemId = table.Column<long>(nullable: false),
                    BookId = table.Column<long>(nullable: false),
                    BookPageNumber = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Note = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(type: "decimal(18, 0)", nullable: false),
                    Quantity = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    UnitId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_Addition",
                        column: x => x.AdditionId,
                        principalTable: "Addition",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_BaseItem",
                        column: x => x.BaseItemId,
                        principalTable: "BaseItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_Book",
                        column: x => x.BookId,
                        principalTable: "Book",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_Currency",
                        column: x => x.CurrencyId,
                        principalTable: "Currency",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingStoreItem_Unit",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RobbingStoreItem_AdditionId",
                table: "RobbingStoreItem",
                column: "AdditionId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingStoreItem_BaseItemId",
                table: "RobbingStoreItem",
                column: "BaseItemId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingStoreItem_BookId",
                table: "RobbingStoreItem",
                column: "BookId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingStoreItem_CurrencyId",
                table: "RobbingStoreItem",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingStoreItem_UnitId",
                table: "RobbingStoreItem",
                column: "UnitId");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class exchangeorderstatustracker : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ExchangeOrderStatusTracker",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ExchangeOrderStatusId = table.Column<int>(nullable: false),
                    ExchangeOrderId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOrderStatusTracker", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExchangeOrderStatusTracker_ExchangeOrder",
                        column: x => x.ExchangeOrderId,
                        principalTable: "ExchangeOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExchangeOrderStatusTracker_ExchangeOrderStatus",
                        column: x => x.ExchangeOrderStatusId,
                        principalTable: "ExchangeOrderStatus",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOrderStatusTracker_ExchangeOrderId",
                table: "ExchangeOrderStatusTracker",
                column: "ExchangeOrderId");

            migrationBuilder.CreateIndex(
                name: "IX_ExchangeOrderStatusTracker_ExchangeOrderStatusId",
                table: "ExchangeOrderStatusTracker",
                column: "ExchangeOrderStatusId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExchangeOrderStatusTracker");
        }
    }
}

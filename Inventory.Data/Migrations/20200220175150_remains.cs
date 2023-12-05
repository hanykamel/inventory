using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class remains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Remains",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remains", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RemainsDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    RemainsId = table.Column<Guid>(nullable: false),
                    UnitId = table.Column<int>(nullable: true),
                    Quantity = table.Column<decimal>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    Price = table.Column<decimal>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    RemainsId1 = table.Column<int>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RemainsDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RemainsDetails_Remains_RemainsId1",
                        column: x => x.RemainsId1,
                        principalTable: "Remains",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RemainsDetails_Unit_UnitId",
                        column: x => x.UnitId,
                        principalTable: "Unit",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemainsDetails_RemainsId1",
                table: "RemainsDetails",
                column: "RemainsId1");

            migrationBuilder.CreateIndex(
                name: "IX_RemainsDetails_UnitId",
                table: "RemainsDetails",
                column: "UnitId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RemainsDetails");

            migrationBuilder.DropTable(
                name: "Remains");
        }
    }
}

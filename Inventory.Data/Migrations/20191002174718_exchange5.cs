using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class exchange5 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeOrder_ExchangeOrderStatus",
                table: "ExchangeOrder");

            migrationBuilder.DropTable(
                name: "ExchangeOrderStatus");

            migrationBuilder.RenameColumn(
                name: "ExchangeOrderStatusId",
                table: "ExchangeOrder",
                newName: "ExchangeOrderStatusNewId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeOrder_ExchangeOrderStatusId",
                table: "ExchangeOrder",
                newName: "IX_ExchangeOrder_ExchangeOrderStatusNewId");

            migrationBuilder.CreateTable(
                name: "ExchangeOrderStatusNew",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOrderStatusNew", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeOrder_ExchangeOrderStatusNew",
                table: "ExchangeOrder",
                column: "ExchangeOrderStatusNewId",
                principalTable: "ExchangeOrderStatusNew",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExchangeOrder_ExchangeOrderStatusNew",
                table: "ExchangeOrder");

            migrationBuilder.DropTable(
                name: "ExchangeOrderStatusNew");

            migrationBuilder.RenameColumn(
                name: "ExchangeOrderStatusNewId",
                table: "ExchangeOrder",
                newName: "ExchangeOrderStatusId");

            migrationBuilder.RenameIndex(
                name: "IX_ExchangeOrder_ExchangeOrderStatusNewId",
                table: "ExchangeOrder",
                newName: "IX_ExchangeOrder_ExchangeOrderStatusId");

            migrationBuilder.CreateTable(
                name: "ExchangeOrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExchangeOrderStatus", x => x.Id);
                });

            migrationBuilder.AddForeignKey(
                name: "FK_ExchangeOrder_ExchangeOrderStatus",
                table: "ExchangeOrder",
                column: "ExchangeOrderStatusId",
                principalTable: "ExchangeOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

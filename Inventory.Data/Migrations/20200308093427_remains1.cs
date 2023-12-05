using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class remains1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "RemainsId",
                table: "RemainsDetails",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<long>(
                name: "RemainsId",
                table: "ExecutionOrderResultRemain",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.CreateTable(
                name: "Remains",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: true),
                    Description = table.Column<string>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remains", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemainsDetails_RemainsId",
                table: "RemainsDetails",
                column: "RemainsId");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultRemain_RemainsId",
                table: "ExecutionOrderResultRemain",
                column: "RemainsId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultRemain_Remains_RemainsId",
                table: "ExecutionOrderResultRemain",
                column: "RemainsId",
                principalTable: "Remains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_RemainsDetails_Remains_RemainsId",
                table: "RemainsDetails",
                column: "RemainsId",
                principalTable: "Remains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultRemain_Remains_RemainsId",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropForeignKey(
                name: "FK_RemainsDetails_Remains_RemainsId",
                table: "RemainsDetails");

            migrationBuilder.DropTable(
                name: "Remains");

            migrationBuilder.DropIndex(
                name: "IX_RemainsDetails_RemainsId",
                table: "RemainsDetails");

            migrationBuilder.DropIndex(
                name: "IX_ExecutionOrderResultRemain_RemainsId",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropColumn(
                name: "RemainsId",
                table: "RemainsDetails");

            migrationBuilder.DropColumn(
                name: "RemainsId",
                table: "ExecutionOrderResultRemain");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class executionmodification6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExecutionOrderResultRemain_Remains_RemainsId1",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropForeignKey(
                name: "FK_RemainsDetails_Remains_RemainsId1",
                table: "RemainsDetails");

            migrationBuilder.DropTable(
                name: "Remains");

            migrationBuilder.DropIndex(
                name: "IX_RemainsDetails_RemainsId1",
                table: "RemainsDetails");

            migrationBuilder.DropIndex(
                name: "IX_ExecutionOrderResultRemain_RemainsId1",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropColumn(
                name: "RemainsId",
                table: "RemainsDetails");

            migrationBuilder.DropColumn(
                name: "RemainsId1",
                table: "RemainsDetails");

            migrationBuilder.DropColumn(
                name: "RemainsId",
                table: "ExecutionOrderResultRemain");

            migrationBuilder.DropColumn(
                name: "RemainsId1",
                table: "ExecutionOrderResultRemain");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "RemainsId",
                table: "RemainsDetails",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "RemainsId1",
                table: "RemainsDetails",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RemainsId",
                table: "ExecutionOrderResultRemain",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "RemainsId1",
                table: "ExecutionOrderResultRemain",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Remains",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Description = table.Column<string>(nullable: true),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Remains", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RemainsDetails_RemainsId1",
                table: "RemainsDetails",
                column: "RemainsId1");

            migrationBuilder.CreateIndex(
                name: "IX_ExecutionOrderResultRemain_RemainsId1",
                table: "ExecutionOrderResultRemain",
                column: "RemainsId1");

            migrationBuilder.AddForeignKey(
                name: "FK_ExecutionOrderResultRemain_Remains_RemainsId1",
                table: "ExecutionOrderResultRemain",
                column: "RemainsId1",
                principalTable: "Remains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RemainsDetails_Remains_RemainsId1",
                table: "RemainsDetails",
                column: "RemainsId1",
                principalTable: "Remains",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

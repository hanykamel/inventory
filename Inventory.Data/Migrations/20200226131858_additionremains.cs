using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class additionremains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "AdditionId",
                table: "RemainsDetails",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_RemainsDetails_AdditionId",
                table: "RemainsDetails",
                column: "AdditionId");

            migrationBuilder.AddForeignKey(
                name: "FK_RemainsDetails_Addition_AdditionId",
                table: "RemainsDetails",
                column: "AdditionId",
                principalTable: "Addition",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RemainsDetails_Addition_AdditionId",
                table: "RemainsDetails");

            migrationBuilder.DropIndex(
                name: "IX_RemainsDetails_AdditionId",
                table: "RemainsDetails");

            migrationBuilder.DropColumn(
                name: "AdditionId",
                table: "RemainsDetails");
        }
    }
}

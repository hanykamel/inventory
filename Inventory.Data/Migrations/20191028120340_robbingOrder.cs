using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class robbingOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobbingOrder_Employees_ForEmployeeId",
                table: "RobbingOrder");

            migrationBuilder.RenameColumn(
                name: "ForEmployeeId",
                table: "RobbingOrder",
                newName: "FromStoreId");

            migrationBuilder.RenameColumn(
                name: "DirectOrderNotes",
                table: "RobbingOrder",
                newName: "RequesterName");

            migrationBuilder.RenameIndex(
                name: "IX_RobbingOrder_ForEmployeeId",
                table: "RobbingOrder",
                newName: "IX_RobbingOrder_FromStoreId");

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "RobbingOrder",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "ToStoreId",
                table: "RobbingOrder",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_RobbingOrder_Store",
                table: "RobbingOrder",
                column: "FromStoreId",
                principalTable: "Store",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RobbingOrder_Store",
                table: "RobbingOrder");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "RobbingOrder");

            migrationBuilder.DropColumn(
                name: "ToStoreId",
                table: "RobbingOrder");

            migrationBuilder.RenameColumn(
                name: "RequesterName",
                table: "RobbingOrder",
                newName: "DirectOrderNotes");

            migrationBuilder.RenameColumn(
                name: "FromStoreId",
                table: "RobbingOrder",
                newName: "ForEmployeeId");

            migrationBuilder.RenameIndex(
                name: "IX_RobbingOrder_FromStoreId",
                table: "RobbingOrder",
                newName: "IX_RobbingOrder_ForEmployeeId");

            migrationBuilder.AddForeignKey(
                name: "FK_RobbingOrder_Employees_ForEmployeeId",
                table: "RobbingOrder",
                column: "ForEmployeeId",
                principalTable: "Employees",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

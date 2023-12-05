using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class delegationshift : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ShiftId",
                table: "Delegation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_ShiftId",
                table: "Delegation",
                column: "ShiftId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delegation_Shift_ShiftId",
                table: "Delegation",
                column: "ShiftId",
                principalTable: "Shift",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_Shift_ShiftId",
                table: "Delegation");

            migrationBuilder.DropIndex(
                name: "IX_Delegation_ShiftId",
                table: "Delegation");

            migrationBuilder.DropColumn(
                name: "ShiftId",
                table: "Delegation");
        }
    }
}

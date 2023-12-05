using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class delegationtype : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "DelegationTypeId",
                table: "Delegation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "DelegationType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DelegationType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Delegation_DelegationTypeId",
                table: "Delegation",
                column: "DelegationTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Delegation_DelegationType",
                table: "Delegation",
                column: "DelegationTypeId",
                principalTable: "DelegationType",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Delegation_DelegationType",
                table: "Delegation");

            migrationBuilder.DropTable(
                name: "DelegationType");

            migrationBuilder.DropIndex(
                name: "IX_Delegation_DelegationTypeId",
                table: "Delegation");

            migrationBuilder.DropColumn(
                name: "DelegationTypeId",
                table: "Delegation");
        }
    }
}

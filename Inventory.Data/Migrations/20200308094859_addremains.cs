using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class addremains : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Remains",
                columns: table => new
                {
                    Id = table.Column<long>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
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
        }
    }
}

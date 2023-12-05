using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class currency_identity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Currency",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "Currency",
                table: "ExaminationCommitte");

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "CurrencyId",
                table: "ExaminationCommitte",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Currency",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    Name = table.Column<string>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Currency", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StoreItem_CurrencyId",
                table: "StoreItem",
                column: "CurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_ExaminationCommitte_CurrencyId",
                table: "ExaminationCommitte",
                column: "CurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_ExaminationCommitte_Currency_CurrencyId",
                table: "ExaminationCommitte",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StoreItem_Currency_CurrencyId",
                table: "StoreItem",
                column: "CurrencyId",
                principalTable: "Currency",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ExaminationCommitte_Currency_CurrencyId",
                table: "ExaminationCommitte");

            migrationBuilder.DropForeignKey(
                name: "FK_StoreItem_Currency_CurrencyId",
                table: "StoreItem");

            migrationBuilder.DropTable(
                name: "Currency");

            migrationBuilder.DropIndex(
                name: "IX_StoreItem_CurrencyId",
                table: "StoreItem");

            migrationBuilder.DropIndex(
                name: "IX_ExaminationCommitte_CurrencyId",
                table: "ExaminationCommitte");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "CurrencyId",
                table: "ExaminationCommitte");

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Currency",
                table: "ExaminationCommitte",
                nullable: true);
        }
    }
}

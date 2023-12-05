using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class additioncommittee : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addition_ExaminationCommitte",
                table: "Addition");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExaminationCommitteId",
                table: "Addition",
                nullable: false,
                oldClrType: typeof(Guid),
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Addition_ExaminationCommitte",
                table: "Addition",
                column: "ExaminationCommitteId",
                principalTable: "ExaminationCommitte",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addition_ExaminationCommitte",
                table: "Addition");

            migrationBuilder.AlterColumn<Guid>(
                name: "ExaminationCommitteId",
                table: "Addition",
                nullable: true,
                oldClrType: typeof(Guid));

            migrationBuilder.AddForeignKey(
                name: "FK_Addition_ExaminationCommitte",
                table: "Addition",
                column: "ExaminationCommitteId",
                principalTable: "ExaminationCommitte",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

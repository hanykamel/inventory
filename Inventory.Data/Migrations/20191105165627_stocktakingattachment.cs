using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class stocktakingattachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StockTakingAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    AttachmentId = table.Column<Guid>(nullable: false),
                    StockTakingId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StockTakingAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StockTakingAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StockTakingAttachment_StockTaking",
                        column: x => x.StockTakingId,
                        principalTable: "StockTaking",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StockTakingAttachment_AttachmentId",
                table: "StockTakingAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StockTakingAttachment_StockTakingId",
                table: "StockTakingAttachment",
                column: "StockTakingId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StockTakingAttachment");
        }
    }
}

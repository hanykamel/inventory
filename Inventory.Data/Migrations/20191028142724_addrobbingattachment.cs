using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class addrobbingattachment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RobbingOrderAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    RobbingOrderId = table.Column<Guid>(nullable: false),
                    AttachmentId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingOrderAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RobbingOrderAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_RobbingOrderAttachment_RobbingOrder",
                        column: x => x.RobbingOrderId,
                        principalTable: "RobbingOrder",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrderAttachment_AttachmentId",
                table: "RobbingOrderAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrderAttachment_RobbingOrderId",
                table: "RobbingOrderAttachment",
                column: "RobbingOrderId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RobbingOrderAttachment");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class notificationhandler1 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "NotificationHandler");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "NotificationHandler",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    HandlerId = table.Column<int>(nullable: false),
                    IsActive = table.Column<bool>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    ModifiedBy = table.Column<string>(nullable: true),
                    NotificationTemplateId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_NotificationHandler", x => x.Id);
                    table.ForeignKey(
                        name: "FK_NotificationHandler_NotificationTemplate",
                        column: x => x.NotificationTemplateId,
                        principalTable: "NotificationTemplate",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_NotificationHandler_NotificationTemplateId",
                table: "NotificationHandler",
                column: "NotificationTemplateId");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class notificationTemplate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "NotificationTemplateId",
                table: "Notification",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Notification_NotificationTemplateId",
                table: "Notification",
                column: "NotificationTemplateId");

            migrationBuilder.AddForeignKey(
                name: "FK_Notification_NotificationTemplate",
                table: "Notification",
                column: "NotificationTemplateId",
                principalTable: "NotificationTemplate",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Notification_NotificationTemplate",
                table: "Notification");

            migrationBuilder.DropIndex(
                name: "IX_Notification_NotificationTemplateId",
                table: "Notification");

            migrationBuilder.DropColumn(
                name: "NotificationTemplateId",
                table: "Notification");
        }
    }
}

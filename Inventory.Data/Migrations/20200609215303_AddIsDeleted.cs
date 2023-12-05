using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class AddIsDeleted : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "CommitteeItem",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "CommitteeEmployee",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "CommitteeAttachment",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Attachment",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDelete",
                table: "Addition",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "StoreItem");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "CommitteeItem");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "CommitteeEmployee");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "CommitteeAttachment");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Attachment");

            migrationBuilder.DropColumn(
                name: "IsDelete",
                table: "Addition");
        }
    }
}

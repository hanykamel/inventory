using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class exchang_directNotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DirectOrderNotes",
                table: "ExchangeOrder",
                nullable: true,
                oldClrType: typeof(string));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "DirectOrderNotes",
                table: "ExchangeOrder",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);
        }
    }
}

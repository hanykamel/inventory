using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class history : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StoreItemCopy",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    AdditionId = table.Column<Guid>(nullable: false),
                    BaseItemId = table.Column<long>(nullable: false),
                    Code = table.Column<string>(nullable: true),
                    Price = table.Column<decimal>(nullable: false),
                    BookId = table.Column<long>(nullable: false),
                    BookPageNumber = table.Column<int>(nullable: false),
                    Note = table.Column<string>(nullable: true),
                    StoreId = table.Column<int>(nullable: false),
                    RobbingName = table.Column<string>(nullable: true),
                    RobbingPrice = table.Column<decimal>(nullable: true),
                    UnitId = table.Column<int>(nullable: true),
                    StoreItemStatusId = table.Column<int>(nullable: false),
                    CurrentItemStatusId = table.Column<int>(nullable: false),
                    IsStagnant = table.Column<bool>(nullable: true),
                    UnderDelete = table.Column<bool>(nullable: true),
                    Serial = table.Column<int>(nullable: false),
                    TenantId = table.Column<int>(nullable: false),
                    CurrencyId = table.Column<int>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    AuditAction = table.Column<string>(nullable: true),
                    HistoryId = table.Column<Guid>(nullable: false),
                    AuditUser = table.Column<string>(nullable: true),
                    AuditDate = table.Column<DateTime>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StoreItemCopy", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StoreItemCopy");
        }
    }
}

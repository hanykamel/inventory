using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class stagnant : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsStagnant",
                table: "StoreItem",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Stagnant",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    DateFrom = table.Column<DateTime>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stagnant", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StagnantAttachment",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    AttachmentId = table.Column<Guid>(nullable: false),
                    StagnantId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagnantAttachment", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StagnantAttachment_Attachment",
                        column: x => x.AttachmentId,
                        principalTable: "Attachment",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StagnantAttachment_Stagnant",
                        column: x => x.StagnantId,
                        principalTable: "Stagnant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "StagnantStoreItem",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    StagnantId = table.Column<Guid>(nullable: false),
                    StoreItemId = table.Column<Guid>(nullable: false),
                    TenantId = table.Column<int>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StagnantStoreItem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StagnantStoreItem_Stagnant",
                        column: x => x.StagnantId,
                        principalTable: "Stagnant",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StagnantStoreItem_StoreItem",
                        column: x => x.StoreItemId,
                        principalTable: "StoreItem",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StagnantAttachment_AttachmentId",
                table: "StagnantAttachment",
                column: "AttachmentId");

            migrationBuilder.CreateIndex(
                name: "IX_StagnantAttachment_StagnantId",
                table: "StagnantAttachment",
                column: "StagnantId");

            migrationBuilder.CreateIndex(
                name: "IX_StagnantStoreItem_StagnantId",
                table: "StagnantStoreItem",
                column: "StagnantId");

            migrationBuilder.CreateIndex(
                name: "IX_StagnantStoreItem_StoreItemId",
                table: "StagnantStoreItem",
                column: "StoreItemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StagnantAttachment");

            migrationBuilder.DropTable(
                name: "StagnantStoreItem");

            migrationBuilder.DropTable(
                name: "Stagnant");

            migrationBuilder.DropColumn(
                name: "IsStagnant",
                table: "StoreItem");
        }
    }
}

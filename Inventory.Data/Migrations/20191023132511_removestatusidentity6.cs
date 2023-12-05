using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class removestatusidentity6 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "RefundOrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RefundOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RobbingOrderStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    CreatedBy = table.Column<string>(nullable: true),
                    CreationDate = table.Column<DateTime>(nullable: false),
                    ModifiedBy = table.Column<string>(nullable: true),
                    ModificationDate = table.Column<DateTime>(nullable: true),
                    IsActive = table.Column<bool>(nullable: true),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RobbingOrderStatus", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "TransformationStatus",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false),
                    Name = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TransformationStatus", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Transformation_TransformationStatusId",
                table: "Transformation",
                column: "TransformationStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RobbingOrder_RobbingOrderStatusId",
                table: "RobbingOrder",
                column: "RobbingOrderStatusId");

            migrationBuilder.CreateIndex(
                name: "IX_RefundOrder_RefundOrderStatusId",
                table: "RefundOrder",
                column: "RefundOrderStatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrder_RefundOrderStatus",
                table: "RefundOrder",
                column: "RefundOrderStatusId",
                principalTable: "RefundOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RobbingOrder_RobbingOrderStatus",
                table: "RobbingOrder",
                column: "RobbingOrderStatusId",
                principalTable: "RobbingOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transformation_TransformationStatus",
                table: "Transformation",
                column: "TransformationStatusId",
                principalTable: "TransformationStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrder_RefundOrderStatus",
                table: "RefundOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_RobbingOrder_RobbingOrderStatus",
                table: "RobbingOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Transformation_TransformationStatus",
                table: "Transformation");

            migrationBuilder.DropTable(
                name: "RefundOrderStatus");

            migrationBuilder.DropTable(
                name: "RobbingOrderStatus");

            migrationBuilder.DropTable(
                name: "TransformationStatus");

            migrationBuilder.DropIndex(
                name: "IX_Transformation_TransformationStatusId",
                table: "Transformation");

            migrationBuilder.DropIndex(
                name: "IX_RobbingOrder_RobbingOrderStatusId",
                table: "RobbingOrder");

            migrationBuilder.DropIndex(
                name: "IX_RefundOrder_RefundOrderStatusId",
                table: "RefundOrder");
        }
    }
}

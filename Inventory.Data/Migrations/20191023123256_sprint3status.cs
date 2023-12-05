using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class sprint3status : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransformationStatus",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RobbingOrderStatus",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RefundOrderStatus",
                nullable: true,
                oldClrType: typeof(string));

            migrationBuilder.AlterColumn<int>(
                name: "AdditionDocumentTypeId",
                table: "Addition",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<DateTime>(
                name: "AdditionDocumentDate",
                table: "Addition",
                nullable: true,
                oldClrType: typeof(DateTime));

            migrationBuilder.AddForeignKey(
                name: "FK_RefundOrder_RefundOrderStatus_RefundOrderStatusId",
                table: "RefundOrder",
                column: "RefundOrderStatusId",
                principalTable: "RefundOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_RobbingOrder_RobbingOrderStatus_RobbingOrderStatusId",
                table: "RobbingOrder",
                column: "RobbingOrderStatusId",
                principalTable: "RobbingOrderStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Transformation_TransformationStatus_TransformationStatusId",
                table: "Transformation",
                column: "TransformationStatusId",
                principalTable: "TransformationStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RefundOrder_RefundOrderStatus_RefundOrderStatusId",
                table: "RefundOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_RobbingOrder_RobbingOrderStatus_RobbingOrderStatusId",
                table: "RobbingOrder");

            migrationBuilder.DropForeignKey(
                name: "FK_Transformation_TransformationStatus_TransformationStatusId",
                table: "Transformation");

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "TransformationStatus",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RobbingOrderStatus",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Name",
                table: "RefundOrderStatus",
                nullable: false,
                oldClrType: typeof(string),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "AdditionDocumentTypeId",
                table: "Addition",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<DateTime>(
                name: "AdditionDocumentDate",
                table: "Addition",
                nullable: false,
                oldClrType: typeof(DateTime),
                oldNullable: true);

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
    }
}

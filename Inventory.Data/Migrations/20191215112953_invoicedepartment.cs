using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class invoicedepartment : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "ToStoreId",
                table: "Transformation",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "ToStoreId",
                table: "RobbingOrder",
                nullable: false,
                oldClrType: typeof(int),
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DepartmentId",
                table: "Invoice",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Invoice_DepartmentId",
                table: "Invoice",
                column: "DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Invoice_Departments",
                table: "Invoice",
                column: "DepartmentId",
                principalTable: "Department",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Invoice_Departments",
                table: "Invoice");

            migrationBuilder.DropIndex(
                name: "IX_Invoice_DepartmentId",
                table: "Invoice");

            migrationBuilder.DropColumn(
                name: "DepartmentId",
                table: "Invoice");

            migrationBuilder.AlterColumn<int>(
                name: "ToStoreId",
                table: "Transformation",
                nullable: true,
                oldClrType: typeof(int));

            migrationBuilder.AlterColumn<int>(
                name: "ToStoreId",
                table: "RobbingOrder",
                nullable: true,
                oldClrType: typeof(int));
        }
    }
}

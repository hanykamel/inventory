using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Inventory.Data.Migrations
{
    public partial class transformationrobbingremovecolumns : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AdditionNumber",
                table: "Transformation");

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Transformation");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "Transformation");

            migrationBuilder.DropColumn(
                name: "RequesterName",
                table: "Transformation");

            migrationBuilder.DropColumn(
                name: "AdditionNumber",
                table: "RobbingOrder");

            migrationBuilder.DropColumn(
                name: "RequestDate",
                table: "RobbingOrder");

            migrationBuilder.DropColumn(
                name: "RequesterName",
                table: "RobbingOrder");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "AdditionNumber",
                table: "Transformation",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Transformation",
                type: "date",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "Transformation",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RequesterName",
                table: "Transformation",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "AdditionNumber",
                table: "RobbingOrder",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "RequestDate",
                table: "RobbingOrder",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "RequesterName",
                table: "RobbingOrder",
                nullable: true);
        }
    }
}

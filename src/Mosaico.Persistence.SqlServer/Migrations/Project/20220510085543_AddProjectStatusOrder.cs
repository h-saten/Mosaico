using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class AddProjectStatusOrder : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "prj",
                table: "ProjectStatuses",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.UpdateData(
                schema: "prj",
                table: "ProjectStatuses",
                keyColumn: "Id",
                keyValue: new Guid("5730abcf-134b-4116-b186-0e1f54c1d1c6"),
                column: "Order",
                value: 4);

            migrationBuilder.UpdateData(
                schema: "prj",
                table: "ProjectStatuses",
                keyColumn: "Id",
                keyValue: new Guid("6d150791-925f-4c7e-8f9a-87d31e3aa061"),
                column: "Order",
                value: 5);

            migrationBuilder.UpdateData(
                schema: "prj",
                table: "ProjectStatuses",
                keyColumn: "Id",
                keyValue: new Guid("74246a47-a93d-4713-b8c7-4f51263947ce"),
                column: "Order",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "prj",
                table: "ProjectStatuses",
                keyColumn: "Id",
                keyValue: new Guid("7529ec5c-5351-44f9-bea6-e89d27a3bd23"),
                column: "Order",
                value: 1);

            migrationBuilder.UpdateData(
                schema: "prj",
                table: "ProjectStatuses",
                keyColumn: "Id",
                keyValue: new Guid("9aa58972-d2c8-467d-a162-7ea773e5aded"),
                column: "Order",
                value: 3);

            migrationBuilder.UpdateData(
                schema: "prj",
                table: "ProjectStatuses",
                keyColumn: "Id",
                keyValue: new Guid("fc4982ea-5b11-4a75-a2d4-4d192ba42848"),
                column: "Order",
                value: 2);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Order",
                schema: "prj",
                table: "ProjectStatuses");
        }
    }
}

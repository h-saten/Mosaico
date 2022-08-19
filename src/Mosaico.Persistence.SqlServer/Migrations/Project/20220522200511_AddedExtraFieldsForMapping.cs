using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class AddedExtraFieldsForMapping : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsBlockedForEditing",
                schema: "prj",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "Order",
                schema: "prj",
                table: "Projects",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartDate",
                schema: "prj",
                table: "Projects",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsBlockedForEditing",
                schema: "prj",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "Order",
                schema: "prj",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "StartDate",
                schema: "prj",
                table: "Projects");
        }
    }
}

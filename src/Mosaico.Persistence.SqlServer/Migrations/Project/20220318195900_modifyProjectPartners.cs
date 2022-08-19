using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class modifyProjectPartners : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                schema: "prj",
                table: "PagePartners",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PagePartners_ProjectId",
                schema: "prj",
                table: "PagePartners",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_PagePartners_Projects_ProjectId",
                schema: "prj",
                table: "PagePartners",
                column: "ProjectId",
                principalSchema: "prj",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PagePartners_Projects_ProjectId",
                schema: "prj",
                table: "PagePartners");

            migrationBuilder.DropIndex(
                name: "IX_PagePartners_ProjectId",
                schema: "prj",
                table: "PagePartners");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                schema: "prj",
                table: "PagePartners");
        }
    }
}

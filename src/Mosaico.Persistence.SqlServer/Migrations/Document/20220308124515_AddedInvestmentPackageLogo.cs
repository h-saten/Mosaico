using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Document
{
    public partial class AddedInvestmentPackageLogo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Documents_PageId",
                schema: "doc",
                table: "Documents");

            migrationBuilder.AddColumn<Guid>(
                name: "InvestmentPackageId",
                schema: "doc",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "PageCover_PageId",
                schema: "doc",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PageCover_PageId",
                schema: "doc",
                table: "Documents",
                column: "PageCover_PageId",
                unique: true,
                filter: "[PageCover_PageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PageId_InvestmentPackageId",
                schema: "doc",
                table: "Documents",
                columns: new[] { "PageId", "InvestmentPackageId" },
                unique: true,
                filter: "[PageId] IS NOT NULL AND [InvestmentPackageId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Documents_PageCover_PageId",
                schema: "doc",
                table: "Documents");

            migrationBuilder.DropIndex(
                name: "IX_Documents_PageId_InvestmentPackageId",
                schema: "doc",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "InvestmentPackageId",
                schema: "doc",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "PageCover_PageId",
                schema: "doc",
                table: "Documents");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PageId",
                schema: "doc",
                table: "Documents",
                column: "PageId",
                unique: true,
                filter: "[PageId] IS NOT NULL");
        }
    }
}

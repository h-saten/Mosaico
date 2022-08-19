using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Document
{
    public partial class AddedStakingTerms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StakingPairId",
                schema: "doc",
                table: "Documents",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_StakingPairId",
                schema: "doc",
                table: "Documents",
                column: "StakingPairId",
                unique: true,
                filter: "[StakingPairId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Documents_StakingPairId",
                schema: "doc",
                table: "Documents");

            migrationBuilder.DropColumn(
                name: "StakingPairId",
                schema: "doc",
                table: "Documents");
        }
    }
}

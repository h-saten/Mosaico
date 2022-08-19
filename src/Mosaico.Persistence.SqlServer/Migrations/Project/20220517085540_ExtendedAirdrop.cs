using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class ExtendedAirdrop : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "CountAsPurchase",
                schema: "prj",
                table: "AirdropCampaigns",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "StageId",
                schema: "prj",
                table: "AirdropCampaigns",
                type: "uniqueidentifier",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CountAsPurchase",
                schema: "prj",
                table: "AirdropCampaigns");

            migrationBuilder.DropColumn(
                name: "StageId",
                schema: "prj",
                table: "AirdropCampaigns");
        }
    }
}

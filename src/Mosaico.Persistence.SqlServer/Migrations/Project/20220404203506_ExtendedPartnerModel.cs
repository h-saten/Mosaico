using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class ExtendedPartnerModel : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TokenFeeAmount",
                schema: "prj",
                table: "PartnerTransactions",
                newName: "RewardAmount");

            migrationBuilder.AddColumn<string>(
                name: "RewardClaimTransactionHash",
                schema: "prj",
                table: "PartnerTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RewardClaimed",
                schema: "prj",
                table: "PartnerTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RewardClaimedAt",
                schema: "prj",
                table: "PartnerTransactions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "prj",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RewardClaimTransactionHash",
                schema: "prj",
                table: "PartnerTransactions");

            migrationBuilder.DropColumn(
                name: "RewardClaimed",
                schema: "prj",
                table: "PartnerTransactions");

            migrationBuilder.DropColumn(
                name: "RewardClaimedAt",
                schema: "prj",
                table: "PartnerTransactions");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "prj",
                table: "Partners");

            migrationBuilder.RenameColumn(
                name: "RewardAmount",
                schema: "prj",
                table: "PartnerTransactions",
                newName: "TokenFeeAmount");
        }
    }
}

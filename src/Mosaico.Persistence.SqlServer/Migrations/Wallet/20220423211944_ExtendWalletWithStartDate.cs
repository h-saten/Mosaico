using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class ExtendWalletWithStartDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Stakings_TokenId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "StakingId",
                schema: "wlt",
                table: "Tokens");

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartsAt",
                schema: "wlt",
                table: "Vestings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TargetType",
                schema: "wlt",
                table: "VestingFunds",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartsAt",
                schema: "wlt",
                table: "Stakings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_VestingFunds_TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds",
                column: "TokenDistributionId",
                unique: true,
                filter: "[TokenDistributionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Stakings_TokenId",
                schema: "wlt",
                table: "Stakings",
                column: "TokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_VestingFunds_TokenDistributions_TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds",
                column: "TokenDistributionId",
                principalSchema: "wlt",
                principalTable: "TokenDistributions",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VestingFunds_TokenDistributions_TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropIndex(
                name: "IX_VestingFunds_TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropIndex(
                name: "IX_Stakings_TokenId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "StartsAt",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "TargetType",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "StartsAt",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.AddColumn<Guid>(
                name: "StakingId",
                schema: "wlt",
                table: "Tokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stakings_TokenId",
                schema: "wlt",
                table: "Stakings",
                column: "TokenId",
                unique: true);
        }
    }
}

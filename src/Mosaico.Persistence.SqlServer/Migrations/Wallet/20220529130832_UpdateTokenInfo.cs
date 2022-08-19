using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class UpdateTokenInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vestings_Tokens_TokenId",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "VestingId",
                schema: "wlt",
                table: "Tokens");

            migrationBuilder.AddColumn<bool>(
                name: "IsDeflationary",
                schema: "wlt",
                table: "Tokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsStakingEnabled",
                schema: "wlt",
                table: "Tokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IsVestingEnabled",
                schema: "wlt",
                table: "Tokens",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StakingStartsAt",
                schema: "wlt",
                table: "Tokens",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vestings_Tokens_TokenId",
                schema: "wlt",
                table: "Vestings",
                column: "TokenId",
                principalSchema: "wlt",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vestings_Tokens_TokenId",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "IsDeflationary",
                schema: "wlt",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "IsStakingEnabled",
                schema: "wlt",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "IsVestingEnabled",
                schema: "wlt",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "StakingStartsAt",
                schema: "wlt",
                table: "Tokens");

            migrationBuilder.AddColumn<Guid>(
                name: "VestingId",
                schema: "wlt",
                table: "Tokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Vestings_Tokens_TokenId",
                schema: "wlt",
                table: "Vestings",
                column: "TokenId",
                principalSchema: "wlt",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

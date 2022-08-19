using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedStakingCronExpression : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenLocks_Tokens_TokenId",
                schema: "wlt",
                table: "TokenLocks");

            migrationBuilder.AlterColumn<Guid>(
                name: "TokenId",
                schema: "wlt",
                table: "TokenLocks",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "PaymentCurrencyId",
                schema: "wlt",
                table: "TokenLocks",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CronSchedule",
                schema: "wlt",
                table: "StakingPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_TokenLocks_PaymentCurrencyId",
                schema: "wlt",
                table: "TokenLocks",
                column: "PaymentCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_TokenLocks_PaymentCurrencies_PaymentCurrencyId",
                schema: "wlt",
                table: "TokenLocks",
                column: "PaymentCurrencyId",
                principalSchema: "wlt",
                principalTable: "PaymentCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_TokenLocks_Tokens_TokenId",
                schema: "wlt",
                table: "TokenLocks",
                column: "TokenId",
                principalSchema: "wlt",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenLocks_PaymentCurrencies_PaymentCurrencyId",
                schema: "wlt",
                table: "TokenLocks");

            migrationBuilder.DropForeignKey(
                name: "FK_TokenLocks_Tokens_TokenId",
                schema: "wlt",
                table: "TokenLocks");

            migrationBuilder.DropIndex(
                name: "IX_TokenLocks_PaymentCurrencyId",
                schema: "wlt",
                table: "TokenLocks");

            migrationBuilder.DropColumn(
                name: "PaymentCurrencyId",
                schema: "wlt",
                table: "TokenLocks");

            migrationBuilder.DropColumn(
                name: "CronSchedule",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.AlterColumn<Guid>(
                name: "TokenId",
                schema: "wlt",
                table: "TokenLocks",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TokenLocks_Tokens_TokenId",
                schema: "wlt",
                table: "TokenLocks",
                column: "TokenId",
                principalSchema: "wlt",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

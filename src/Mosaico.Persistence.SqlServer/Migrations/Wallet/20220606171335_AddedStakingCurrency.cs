using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedStakingCurrency : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<Guid>(
                name: "StakingTokenId",
                schema: "wlt",
                table: "StakingPairs",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<Guid>(
                name: "StakingPaymentCurrencyId",
                schema: "wlt",
                table: "StakingPairs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "Type",
                schema: "wlt",
                table: "StakingPairs",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_StakingPairs_StakingPaymentCurrencyId",
                schema: "wlt",
                table: "StakingPairs",
                column: "StakingPaymentCurrencyId");

            migrationBuilder.AddForeignKey(
                name: "FK_StakingPairs_PaymentCurrencies_StakingPaymentCurrencyId",
                schema: "wlt",
                table: "StakingPairs",
                column: "StakingPaymentCurrencyId",
                principalSchema: "wlt",
                principalTable: "PaymentCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StakingPairs_PaymentCurrencies_StakingPaymentCurrencyId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropIndex(
                name: "IX_StakingPairs_StakingPaymentCurrencyId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropColumn(
                name: "StakingPaymentCurrencyId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropColumn(
                name: "Type",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.AlterColumn<Guid>(
                name: "StakingTokenId",
                schema: "wlt",
                table: "StakingPairs",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);
        }
    }
}

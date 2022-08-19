using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class UpdatedStakingEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stakings_PaymentCurrencies_RewardCurrencyId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropForeignKey(
                name: "FK_Stakings_Tokens_RewardTokenId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropForeignKey(
                name: "FK_Stakings_Tokens_TokenId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropIndex(
                name: "IX_Stakings_RewardCurrencyId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropIndex(
                name: "IX_Stakings_RewardTokenId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "CycleLimit",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "MaxParticipants",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "MaxReward",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "RewardCurrencyId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "RewardTokenId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "StartsAt",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.RenameColumn(
                name: "RewardCycle",
                schema: "wlt",
                table: "Stakings",
                newName: "Days");

            migrationBuilder.RenameColumn(
                name: "ContractVersion",
                schema: "wlt",
                table: "Stakings",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "ContractAddress",
                schema: "wlt",
                table: "Stakings",
                newName: "FailureReason");

            migrationBuilder.AlterColumn<Guid>(
                name: "TokenId",
                schema: "wlt",
                table: "Stakings",
                type: "uniqueidentifier",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier");

            migrationBuilder.AddColumn<decimal>(
                name: "Balance",
                schema: "wlt",
                table: "Stakings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "StakingPairId",
                schema: "wlt",
                table: "Stakings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Stakings_StakingPairId",
                schema: "wlt",
                table: "Stakings",
                column: "StakingPairId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stakings_StakingPairs_StakingPairId",
                schema: "wlt",
                table: "Stakings",
                column: "StakingPairId",
                principalSchema: "wlt",
                principalTable: "StakingPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Stakings_Tokens_TokenId",
                schema: "wlt",
                table: "Stakings",
                column: "TokenId",
                principalSchema: "wlt",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Stakings_StakingPairs_StakingPairId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropForeignKey(
                name: "FK_Stakings_Tokens_TokenId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropIndex(
                name: "IX_Stakings_StakingPairId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "Balance",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "StakingPairId",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "wlt",
                table: "Stakings",
                newName: "ContractVersion");

            migrationBuilder.RenameColumn(
                name: "FailureReason",
                schema: "wlt",
                table: "Stakings",
                newName: "ContractAddress");

            migrationBuilder.RenameColumn(
                name: "Days",
                schema: "wlt",
                table: "Stakings",
                newName: "RewardCycle");

            migrationBuilder.AlterColumn<Guid>(
                name: "TokenId",
                schema: "wlt",
                table: "Stakings",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uniqueidentifier",
                oldNullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "CycleLimit",
                schema: "wlt",
                table: "Stakings",
                type: "decimal(3,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<long>(
                name: "MaxParticipants",
                schema: "wlt",
                table: "Stakings",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<decimal>(
                name: "MaxReward",
                schema: "wlt",
                table: "Stakings",
                type: "decimal(38,18)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "RewardCurrencyId",
                schema: "wlt",
                table: "Stakings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "RewardTokenId",
                schema: "wlt",
                table: "Stakings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "StartsAt",
                schema: "wlt",
                table: "Stakings",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Stakings_RewardCurrencyId",
                schema: "wlt",
                table: "Stakings",
                column: "RewardCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_Stakings_RewardTokenId",
                schema: "wlt",
                table: "Stakings",
                column: "RewardTokenId");

            migrationBuilder.AddForeignKey(
                name: "FK_Stakings_PaymentCurrencies_RewardCurrencyId",
                schema: "wlt",
                table: "Stakings",
                column: "RewardCurrencyId",
                principalSchema: "wlt",
                principalTable: "PaymentCurrencies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stakings_Tokens_RewardTokenId",
                schema: "wlt",
                table: "Stakings",
                column: "RewardTokenId",
                principalSchema: "wlt",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Stakings_Tokens_TokenId",
                schema: "wlt",
                table: "Stakings",
                column: "TokenId",
                principalSchema: "wlt",
                principalTable: "Tokens",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

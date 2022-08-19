using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedStakingPairs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Decimals",
                schema: "wlt",
                table: "Tokens",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "StakingPairs",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StakingTokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CanChangeStakingPeriod = table.Column<bool>(type: "bit", nullable: false),
                    MinimumDaysToStake = table.Column<int>(type: "int", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    EstimatedAPR = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    EstimatedRewardInUSD = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    RewardPayedOnDay = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StakingPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StakingPairs_Tokens_StakingTokenId",
                        column: x => x.StakingTokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_StakingPairs_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PaymentCurrencyToStakingPairs",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StakingPairId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentCurrencyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentCurrencyToStakingPairs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PaymentCurrencyToStakingPairs_PaymentCurrencies_PaymentCurrencyId",
                        column: x => x.PaymentCurrencyId,
                        principalSchema: "wlt",
                        principalTable: "PaymentCurrencies",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PaymentCurrencyToStakingPairs_StakingPairs_StakingPairId",
                        column: x => x.StakingPairId,
                        principalSchema: "wlt",
                        principalTable: "StakingPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCurrencyToStakingPairs_PaymentCurrencyId",
                schema: "wlt",
                table: "PaymentCurrencyToStakingPairs",
                column: "PaymentCurrencyId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentCurrencyToStakingPairs_StakingPairId",
                schema: "wlt",
                table: "PaymentCurrencyToStakingPairs",
                column: "StakingPairId");

            migrationBuilder.CreateIndex(
                name: "IX_StakingPairs_StakingTokenId",
                schema: "wlt",
                table: "StakingPairs",
                column: "StakingTokenId");

            migrationBuilder.CreateIndex(
                name: "IX_StakingPairs_TokenId",
                schema: "wlt",
                table: "StakingPairs",
                column: "TokenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PaymentCurrencyToStakingPairs",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "StakingPairs",
                schema: "wlt");

            migrationBuilder.DropColumn(
                name: "Decimals",
                schema: "wlt",
                table: "Tokens");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AdjustedVestingWallet : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_WalletToVesting_Wallets_WalletId1",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropIndex(
                name: "IX_WalletToVesting_VestingId",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropIndex(
                name: "IX_WalletToVesting_WalletId1",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "WalletId1",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.CreateIndex(
                name: "IX_WalletToVesting_VestingId",
                schema: "wlt",
                table: "WalletToVesting",
                column: "VestingId",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_WalletToVesting_VestingId",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId1",
                schema: "wlt",
                table: "WalletToVesting",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_WalletToVesting_VestingId",
                schema: "wlt",
                table: "WalletToVesting",
                column: "VestingId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletToVesting_WalletId1",
                schema: "wlt",
                table: "WalletToVesting",
                column: "WalletId1");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletToVesting_Wallets_WalletId1",
                schema: "wlt",
                table: "WalletToVesting",
                column: "WalletId1",
                principalSchema: "wlt",
                principalTable: "Wallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

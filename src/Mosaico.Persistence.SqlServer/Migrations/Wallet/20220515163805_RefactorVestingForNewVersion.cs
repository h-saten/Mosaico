using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class RefactorVestingForNewVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VestingFunds_TokenDistributions_TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletToVesting_VestingWallets_VestingId",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropTable(
                name: "VestingInvitations",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "VestingWallets",
                schema: "wlt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletToVesting",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropIndex(
                name: "IX_VestingFunds_TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "Claimed",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "CanWithdrawEarly",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "Distribution",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "EndAt",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "ReleaseCycleInDays",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "StageId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "SubtractedPercent",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "VestingWalletId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.RenameColumn(
                name: "TargetType",
                schema: "wlt",
                table: "VestingFunds",
                newName: "Status");

            migrationBuilder.RenameColumn(
                name: "DistributionPerPerson",
                schema: "wlt",
                table: "VestingFunds",
                newName: "TokenAmount");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                schema: "wlt",
                table: "WalletToVesting",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "CreatedAt",
                schema: "wlt",
                table: "WalletToVesting",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "CreatedById",
                schema: "wlt",
                table: "WalletToVesting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ModifiedAt",
                schema: "wlt",
                table: "WalletToVesting",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));

            migrationBuilder.AddColumn<string>(
                name: "ModifiedById",
                schema: "wlt",
                table: "WalletToVesting",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "Version",
                schema: "wlt",
                table: "WalletToVesting",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);

            migrationBuilder.AddColumn<Guid>(
                name: "WalletId1",
                schema: "wlt",
                table: "WalletToVesting",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "InitialPaymentPercentage",
                schema: "wlt",
                table: "Vestings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Name",
                schema: "wlt",
                table: "Vestings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfDays",
                schema: "wlt",
                table: "Vestings",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "TokenAmount",
                schema: "wlt",
                table: "Vestings",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<Guid>(
                name: "VaultId",
                schema: "wlt",
                table: "Vestings",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "WalletAddress",
                schema: "wlt",
                table: "Vestings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "Days",
                schema: "wlt",
                table: "VestingFunds",
                type: "int",
                nullable: false,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddColumn<string>(
                name: "SmartContractId",
                schema: "wlt",
                table: "VestingFunds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletToVesting",
                schema: "wlt",
                table: "WalletToVesting",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_WalletToVesting_WalletId",
                schema: "wlt",
                table: "WalletToVesting",
                column: "WalletId");

            migrationBuilder.CreateIndex(
                name: "IX_WalletToVesting_WalletId1",
                schema: "wlt",
                table: "WalletToVesting",
                column: "WalletId1");

            migrationBuilder.CreateIndex(
                name: "IX_Vestings_VaultId",
                schema: "wlt",
                table: "Vestings",
                column: "VaultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Vestings_Vaults_VaultId",
                schema: "wlt",
                table: "Vestings",
                column: "VaultId",
                principalSchema: "wlt",
                principalTable: "Vaults",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_WalletToVesting_Vestings_VestingId",
                schema: "wlt",
                table: "WalletToVesting",
                column: "VestingId",
                principalSchema: "wlt",
                principalTable: "Vestings",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vestings_Vaults_VaultId",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletToVesting_Vestings_VestingId",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropForeignKey(
                name: "FK_WalletToVesting_Wallets_WalletId1",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropPrimaryKey(
                name: "PK_WalletToVesting",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropIndex(
                name: "IX_WalletToVesting_WalletId",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropIndex(
                name: "IX_WalletToVesting_WalletId1",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropIndex(
                name: "IX_Vestings_VaultId",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "Id",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "CreatedById",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "ModifiedAt",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "ModifiedById",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "Version",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "WalletId1",
                schema: "wlt",
                table: "WalletToVesting");

            migrationBuilder.DropColumn(
                name: "InitialPaymentPercentage",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "Name",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "NumberOfDays",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "TokenAmount",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "VaultId",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "WalletAddress",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.DropColumn(
                name: "SmartContractId",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.RenameColumn(
                name: "TokenAmount",
                schema: "wlt",
                table: "VestingFunds",
                newName: "DistributionPerPerson");

            migrationBuilder.RenameColumn(
                name: "Status",
                schema: "wlt",
                table: "VestingFunds",
                newName: "TargetType");

            migrationBuilder.AddColumn<bool>(
                name: "Claimed",
                schema: "wlt",
                table: "WalletToVesting",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AlterColumn<long>(
                name: "Days",
                schema: "wlt",
                table: "VestingFunds",
                type: "bigint",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<bool>(
                name: "CanWithdrawEarly",
                schema: "wlt",
                table: "VestingFunds",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<decimal>(
                name: "Distribution",
                schema: "wlt",
                table: "VestingFunds",
                type: "decimal(18,2)",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "EndAt",
                schema: "wlt",
                table: "VestingFunds",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "ReleaseCycleInDays",
                schema: "wlt",
                table: "VestingFunds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StageId",
                schema: "wlt",
                table: "VestingFunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<long>(
                name: "SubtractedPercent",
                schema: "wlt",
                table: "VestingFunds",
                type: "bigint",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VestingWalletId",
                schema: "wlt",
                table: "VestingFunds",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_WalletToVesting",
                schema: "wlt",
                table: "WalletToVesting",
                columns: new[] { "WalletId", "VestingId" });

            migrationBuilder.CreateTable(
                name: "VestingInvitations",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AcceptedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    IsInvitationSent = table.Column<bool>(type: "bit", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Phone = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    VestingFundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    WalletAddress = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VestingInvitations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VestingInvitations_VestingFunds_VestingFundId",
                        column: x => x.VestingFundId,
                        principalSchema: "wlt",
                        principalTable: "VestingFunds",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VestingWallets",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AccountAddress = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FundId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PrivateKey = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PublicKey = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    VestingId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VestingWallets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VestingWallets_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_VestingFunds_TokenDistributionId",
                schema: "wlt",
                table: "VestingFunds",
                column: "TokenDistributionId",
                unique: true,
                filter: "[TokenDistributionId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_VestingInvitations_VestingFundId",
                schema: "wlt",
                table: "VestingInvitations",
                column: "VestingFundId");

            migrationBuilder.CreateIndex(
                name: "IX_VestingWallets_TokenId",
                schema: "wlt",
                table: "VestingWallets",
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

            migrationBuilder.AddForeignKey(
                name: "FK_WalletToVesting_VestingWallets_VestingId",
                schema: "wlt",
                table: "WalletToVesting",
                column: "VestingId",
                principalSchema: "wlt",
                principalTable: "VestingWallets",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

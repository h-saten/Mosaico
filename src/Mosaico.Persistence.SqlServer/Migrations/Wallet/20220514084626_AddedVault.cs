using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedVault : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "VaultId",
                schema: "wlt",
                table: "Tokens",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SmartContractId",
                schema: "wlt",
                table: "TokenDistributions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "VaultId",
                schema: "wlt",
                table: "TokenDistributions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Vaults",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Address = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Vaults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Vaults_Tokens_TokenId",
                        column: x => x.TokenId,
                        principalSchema: "wlt",
                        principalTable: "Tokens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_TokenDistributions_VaultId",
                schema: "wlt",
                table: "TokenDistributions",
                column: "VaultId");

            migrationBuilder.CreateIndex(
                name: "IX_Vaults_TokenId",
                schema: "wlt",
                table: "Vaults",
                column: "TokenId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_TokenDistributions_Vaults_VaultId",
                schema: "wlt",
                table: "TokenDistributions",
                column: "VaultId",
                principalSchema: "wlt",
                principalTable: "Vaults",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TokenDistributions_Vaults_VaultId",
                schema: "wlt",
                table: "TokenDistributions");

            migrationBuilder.DropTable(
                name: "Vaults",
                schema: "wlt");

            migrationBuilder.DropIndex(
                name: "IX_TokenDistributions_VaultId",
                schema: "wlt",
                table: "TokenDistributions");

            migrationBuilder.DropColumn(
                name: "VaultId",
                schema: "wlt",
                table: "Tokens");

            migrationBuilder.DropColumn(
                name: "SmartContractId",
                schema: "wlt",
                table: "TokenDistributions");

            migrationBuilder.DropColumn(
                name: "VaultId",
                schema: "wlt",
                table: "TokenDistributions");
        }
    }
}

using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class ExtendedStaking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionHash",
                schema: "wlt",
                table: "StakingClaimHistory",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wallet",
                schema: "wlt",
                table: "StakingClaimHistory",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionHash",
                schema: "wlt",
                table: "StakingClaimHistory");

            migrationBuilder.DropColumn(
                name: "Wallet",
                schema: "wlt",
                table: "StakingClaimHistory");
        }
    }
}

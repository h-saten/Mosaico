using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class ExtendStakingWithMetamask : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionHash",
                schema: "wlt",
                table: "Stakings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Wallet",
                schema: "wlt",
                table: "Stakings",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "WalletType",
                schema: "wlt",
                table: "Stakings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionHash",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "Wallet",
                schema: "wlt",
                table: "Stakings");

            migrationBuilder.DropColumn(
                name: "WalletType",
                schema: "wlt",
                table: "Stakings");
        }
    }
}

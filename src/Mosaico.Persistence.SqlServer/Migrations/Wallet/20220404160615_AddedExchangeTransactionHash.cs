using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedExchangeTransactionHash : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExchangeTransactionHash",
                schema: "wlt",
                table: "Transactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExchangeTransactionHash",
                schema: "wlt",
                table: "Transactions");
        }
    }
}

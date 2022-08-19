using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedVestingDepositTransactionTracking : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApprovalTransactionHash",
                schema: "wlt",
                table: "VestingFunds",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionHash",
                schema: "wlt",
                table: "VestingFunds",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ApprovalTransactionHash",
                schema: "wlt",
                table: "VestingFunds");

            migrationBuilder.DropColumn(
                name: "TransactionHash",
                schema: "wlt",
                table: "VestingFunds");
        }
    }
}

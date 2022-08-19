using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class ExtendStakingPairsWithVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "StakingVersion",
                schema: "wlt",
                table: "StakingPairs",
                type: "nvarchar(max)",
                nullable: true,
                defaultValue: "v1");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StakingVersion",
                schema: "wlt",
                table: "StakingPairs");
        }
    }
}

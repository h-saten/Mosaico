using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Business
{
    public partial class AddAdditionalContractInfo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "IsEverybodyCanVoteEnabled",
                schema: "bsn",
                table: "Companies",
                newName: "OnlyOwnerProposals");

            migrationBuilder.AddColumn<int>(
                name: "InitialVotingDelay",
                schema: "bsn",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "InitialVotingPeriod",
                schema: "bsn",
                table: "Companies",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<long>(
                name: "Quorum",
                schema: "bsn",
                table: "Companies",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "InitialVotingDelay",
                schema: "bsn",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "InitialVotingPeriod",
                schema: "bsn",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "Quorum",
                schema: "bsn",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "OnlyOwnerProposals",
                schema: "bsn",
                table: "Companies",
                newName: "IsEverybodyCanVoteEnabled");
        }
    }
}

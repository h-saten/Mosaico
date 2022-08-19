using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class ExtendAffiliationModelWithConfig : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EverybodyCanParticipate",
                schema: "prj",
                table: "ProjectAffiliations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "IncludeAll",
                schema: "prj",
                table: "ProjectAffiliations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<bool>(
                name: "PartnerShouldBeInvestor",
                schema: "prj",
                table: "ProjectAffiliations",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "PurchasedById",
                schema: "prj",
                table: "PartnerTransactions",
                type: "nvarchar(max)",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EverybodyCanParticipate",
                schema: "prj",
                table: "ProjectAffiliations");

            migrationBuilder.DropColumn(
                name: "IncludeAll",
                schema: "prj",
                table: "ProjectAffiliations");

            migrationBuilder.DropColumn(
                name: "PartnerShouldBeInvestor",
                schema: "prj",
                table: "ProjectAffiliations");

            migrationBuilder.DropColumn(
                name: "PurchasedById",
                schema: "prj",
                table: "PartnerTransactions");
        }
    }
}

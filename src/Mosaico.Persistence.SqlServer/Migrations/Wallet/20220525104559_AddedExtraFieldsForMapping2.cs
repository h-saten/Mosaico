using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedExtraFieldsForMapping2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vestings_TokenId",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.CreateIndex(
                name: "IX_Vestings_TokenId",
                schema: "wlt",
                table: "Vestings",
                column: "TokenId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Vestings_TokenId",
                schema: "wlt",
                table: "Vestings");

            migrationBuilder.CreateIndex(
                name: "IX_Vestings_TokenId",
                schema: "wlt",
                table: "Vestings",
                column: "TokenId",
                unique: true);
        }
    }
}

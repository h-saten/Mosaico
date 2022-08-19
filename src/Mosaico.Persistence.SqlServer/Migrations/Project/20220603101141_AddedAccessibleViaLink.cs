using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class AddedAccessibleViaLink : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsAccessibleViaLink",
                schema: "prj",
                table: "Projects",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsAccessibleViaLink",
                schema: "prj",
                table: "Projects");
        }
    }
}

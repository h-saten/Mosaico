using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Business
{
    public partial class remove_document_table : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Documents_Companies_CompanyId",
                schema: "bsn",
                table: "Documents");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Documents",
                schema: "bsn",
                table: "Documents");

            migrationBuilder.RenameTable(
                name: "Documents",
                schema: "bsn",
                newName: "Document",
                newSchema: "bsn");

            migrationBuilder.RenameIndex(
                name: "IX_Documents_CompanyId",
                schema: "bsn",
                table: "Document",
                newName: "IX_Document_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Document",
                schema: "bsn",
                table: "Document",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Document_Companies_CompanyId",
                schema: "bsn",
                table: "Document",
                column: "CompanyId",
                principalSchema: "bsn",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Document_Companies_CompanyId",
                schema: "bsn",
                table: "Document");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Document",
                schema: "bsn",
                table: "Document");

            migrationBuilder.RenameTable(
                name: "Document",
                schema: "bsn",
                newName: "Documents",
                newSchema: "bsn");

            migrationBuilder.RenameIndex(
                name: "IX_Document_CompanyId",
                schema: "bsn",
                table: "Documents",
                newName: "IX_Documents_CompanyId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Documents",
                schema: "bsn",
                table: "Documents",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Documents_Companies_CompanyId",
                schema: "bsn",
                table: "Documents",
                column: "CompanyId",
                principalSchema: "bsn",
                principalTable: "Companies",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Document
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "doc");

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "doc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Discriminator = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    ArticleCover_ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ArticleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyDocument_CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsMandatory = table.Column<bool>(type: "bit", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ProjectDocument_IsMandatory = table.Column<bool>(type: "bit", nullable: true),
                    ProjectLogo_ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentContents",
                schema: "doc",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FileId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    DocumentAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    DocumentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DocumentBaseId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentContents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DocumentContents_Documents_DocumentBaseId",
                        column: x => x.DocumentBaseId,
                        principalSchema: "doc",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_DocumentContents_Documents_DocumentId",
                        column: x => x.DocumentId,
                        principalSchema: "doc",
                        principalTable: "Documents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_DocumentContents_DocumentBaseId",
                schema: "doc",
                table: "DocumentContents",
                column: "DocumentBaseId");

            migrationBuilder.CreateIndex(
                name: "IX_DocumentContents_DocumentId_Language",
                schema: "doc",
                table: "DocumentContents",
                columns: new[] { "DocumentId", "Language" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ArticleCover_ArticleId",
                schema: "doc",
                table: "Documents",
                column: "ArticleCover_ArticleId",
                unique: true,
                filter: "[ArticleCover_ArticleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ArticleId",
                schema: "doc",
                table: "Documents",
                column: "ArticleId",
                unique: true,
                filter: "[ArticleId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_CompanyId",
                schema: "doc",
                table: "Documents",
                column: "CompanyId",
                unique: true,
                filter: "[CompanyId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_PageId",
                schema: "doc",
                table: "Documents",
                column: "PageId",
                unique: true,
                filter: "[PageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ProjectLogo_ProjectId",
                schema: "doc",
                table: "Documents",
                column: "ProjectLogo_ProjectId",
                unique: true,
                filter: "[ProjectLogo_ProjectId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TokenId",
                schema: "doc",
                table: "Documents",
                column: "TokenId",
                unique: true,
                filter: "[TokenId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_UserId",
                schema: "doc",
                table: "Documents",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "DocumentContents",
                schema: "doc");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "doc");
        }
    }
}

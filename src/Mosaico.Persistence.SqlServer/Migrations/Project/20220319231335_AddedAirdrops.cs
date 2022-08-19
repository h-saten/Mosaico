using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class AddedAirdrops : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "AirdropCampaigns",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    IsOpened = table.Column<bool>(type: "bit", nullable: false),
                    TotalCap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    TokensPerParticipant = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirdropCampaigns", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirdropCampaigns_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AirdropParticipants",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    WalletAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Claimed = table.Column<bool>(type: "bit", nullable: false),
                    ClaimedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    AirdropCampaignId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClaimedTokenAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AirdropParticipants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AirdropParticipants_AirdropCampaigns_AirdropCampaignId",
                        column: x => x.AirdropCampaignId,
                        principalSchema: "prj",
                        principalTable: "AirdropCampaigns",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_AirdropCampaigns_ProjectId",
                schema: "prj",
                table: "AirdropCampaigns",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_AirdropCampaigns_Slug",
                schema: "prj",
                table: "AirdropCampaigns",
                column: "Slug");

            migrationBuilder.CreateIndex(
                name: "IX_AirdropCampaigns_Slug_ProjectId",
                schema: "prj",
                table: "AirdropCampaigns",
                columns: new[] { "Slug", "ProjectId" },
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_AirdropParticipants_AirdropCampaignId",
                schema: "prj",
                table: "AirdropParticipants",
                column: "AirdropCampaignId");

            migrationBuilder.CreateIndex(
                name: "IX_AirdropParticipants_Email_AirdropCampaignId",
                schema: "prj",
                table: "AirdropParticipants",
                columns: new[] { "Email", "AirdropCampaignId" },
                unique: true,
                filter: "[Email] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AirdropParticipants",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "AirdropCampaigns",
                schema: "prj");
        }
    }
}

using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedStakingTerms : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StakingPairs_StakingRegulation_StakingRegulationId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropForeignKey(
                name: "FK_StakingRegulationTranslation_StakingRegulation_StakingRegulationId",
                schema: "wlt",
                table: "StakingRegulationTranslation");

            migrationBuilder.DropIndex(
                name: "IX_StakingPairs_StakingRegulationId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StakingRegulationTranslation",
                schema: "wlt",
                table: "StakingRegulationTranslation");

            migrationBuilder.DropIndex(
                name: "IX_StakingRegulationTranslation_StakingRegulationId",
                schema: "wlt",
                table: "StakingRegulationTranslation");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StakingRegulation",
                schema: "wlt",
                table: "StakingRegulation");

            migrationBuilder.DropColumn(
                name: "TermsAndConditionsUrl",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropColumn(
                name: "StakingRegulationId",
                schema: "wlt",
                table: "StakingRegulationTranslation");

            migrationBuilder.RenameTable(
                name: "StakingRegulationTranslation",
                schema: "wlt",
                newName: "StakingRegulationTranslations",
                newSchema: "wlt");

            migrationBuilder.RenameTable(
                name: "StakingRegulation",
                schema: "wlt",
                newName: "StakingRegulations",
                newSchema: "wlt");

            migrationBuilder.AddColumn<Guid>(
                name: "StakingTermsId",
                schema: "wlt",
                table: "StakingPairs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StakingRegulationTranslations",
                schema: "wlt",
                table: "StakingRegulationTranslations",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StakingRegulations",
                schema: "wlt",
                table: "StakingRegulations",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "StakingTerms",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StakingPairId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StakingTerms", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StakingTerms_StakingPairs_StakingPairId",
                        column: x => x.StakingPairId,
                        principalSchema: "wlt",
                        principalTable: "StakingPairs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StakingTermsTranslations",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StakingTermsTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StakingTermsTranslations_StakingTerms_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "wlt",
                        principalTable: "StakingTerms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StakingRegulationTranslations_EntityId",
                schema: "wlt",
                table: "StakingRegulationTranslations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_StakingRegulations_StakingPairId",
                schema: "wlt",
                table: "StakingRegulations",
                column: "StakingPairId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StakingTerms_StakingPairId",
                schema: "wlt",
                table: "StakingTerms",
                column: "StakingPairId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_StakingTermsTranslations_EntityId",
                schema: "wlt",
                table: "StakingTermsTranslations",
                column: "EntityId");

            migrationBuilder.AddForeignKey(
                name: "FK_StakingRegulations_StakingPairs_StakingPairId",
                schema: "wlt",
                table: "StakingRegulations",
                column: "StakingPairId",
                principalSchema: "wlt",
                principalTable: "StakingPairs",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_StakingRegulationTranslations_StakingRegulations_EntityId",
                schema: "wlt",
                table: "StakingRegulationTranslations",
                column: "EntityId",
                principalSchema: "wlt",
                principalTable: "StakingRegulations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StakingRegulations_StakingPairs_StakingPairId",
                schema: "wlt",
                table: "StakingRegulations");

            migrationBuilder.DropForeignKey(
                name: "FK_StakingRegulationTranslations_StakingRegulations_EntityId",
                schema: "wlt",
                table: "StakingRegulationTranslations");

            migrationBuilder.DropTable(
                name: "StakingTermsTranslations",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "StakingTerms",
                schema: "wlt");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StakingRegulationTranslations",
                schema: "wlt",
                table: "StakingRegulationTranslations");

            migrationBuilder.DropIndex(
                name: "IX_StakingRegulationTranslations_EntityId",
                schema: "wlt",
                table: "StakingRegulationTranslations");

            migrationBuilder.DropPrimaryKey(
                name: "PK_StakingRegulations",
                schema: "wlt",
                table: "StakingRegulations");

            migrationBuilder.DropIndex(
                name: "IX_StakingRegulations_StakingPairId",
                schema: "wlt",
                table: "StakingRegulations");

            migrationBuilder.DropColumn(
                name: "StakingTermsId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.RenameTable(
                name: "StakingRegulationTranslations",
                schema: "wlt",
                newName: "StakingRegulationTranslation",
                newSchema: "wlt");

            migrationBuilder.RenameTable(
                name: "StakingRegulations",
                schema: "wlt",
                newName: "StakingRegulation",
                newSchema: "wlt");

            migrationBuilder.AddColumn<string>(
                name: "TermsAndConditionsUrl",
                schema: "wlt",
                table: "StakingPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "StakingRegulationId",
                schema: "wlt",
                table: "StakingRegulationTranslation",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_StakingRegulationTranslation",
                schema: "wlt",
                table: "StakingRegulationTranslation",
                column: "Id");

            migrationBuilder.AddPrimaryKey(
                name: "PK_StakingRegulation",
                schema: "wlt",
                table: "StakingRegulation",
                column: "Id");

            migrationBuilder.CreateIndex(
                name: "IX_StakingPairs_StakingRegulationId",
                schema: "wlt",
                table: "StakingPairs",
                column: "StakingRegulationId",
                unique: true,
                filter: "[StakingRegulationId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StakingRegulationTranslation_StakingRegulationId",
                schema: "wlt",
                table: "StakingRegulationTranslation",
                column: "StakingRegulationId");

            migrationBuilder.AddForeignKey(
                name: "FK_StakingPairs_StakingRegulation_StakingRegulationId",
                schema: "wlt",
                table: "StakingPairs",
                column: "StakingRegulationId",
                principalSchema: "wlt",
                principalTable: "StakingRegulation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_StakingRegulationTranslation_StakingRegulation_StakingRegulationId",
                schema: "wlt",
                table: "StakingRegulationTranslation",
                column: "StakingRegulationId",
                principalSchema: "wlt",
                principalTable: "StakingRegulation",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}

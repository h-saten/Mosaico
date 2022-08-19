using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class StakingRegulations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "StakingRegulationId",
                schema: "wlt",
                table: "StakingPairs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TermsAndConditionsUrl",
                schema: "wlt",
                table: "StakingPairs",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "StakingRegulation",
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
                    table.PrimaryKey("PK_StakingRegulation", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StakingRegulationTranslation",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StakingRegulationId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
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
                    table.PrimaryKey("PK_StakingRegulationTranslation", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StakingRegulationTranslation_StakingRegulation_StakingRegulationId",
                        column: x => x.StakingRegulationId,
                        principalSchema: "wlt",
                        principalTable: "StakingRegulation",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

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
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_StakingPairs_StakingRegulation_StakingRegulationId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropTable(
                name: "StakingRegulationTranslation",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "StakingRegulation",
                schema: "wlt");

            migrationBuilder.DropIndex(
                name: "IX_StakingPairs_StakingRegulationId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropColumn(
                name: "StakingRegulationId",
                schema: "wlt",
                table: "StakingPairs");

            migrationBuilder.DropColumn(
                name: "TermsAndConditionsUrl",
                schema: "wlt",
                table: "StakingPairs");
        }
    }
}

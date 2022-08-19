using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class RemoveStageJobs : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StageJobs",
                schema: "prj");

            migrationBuilder.AddColumn<bool>(
                name: "AllowRedeployment",
                schema: "prj",
                table: "Stage",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "DeployedAt",
                schema: "prj",
                table: "Stage",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "DeploymentStatus",
                schema: "prj",
                table: "Stage",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AllowRedeployment",
                schema: "prj",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "DeployedAt",
                schema: "prj",
                table: "Stage");

            migrationBuilder.DropColumn(
                name: "DeploymentStatus",
                schema: "prj",
                table: "Stage");

            migrationBuilder.CreateTable(
                name: "StageJobs",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    JobId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageJobs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StageJobs_Stage_StageId",
                        column: x => x.StageId,
                        principalSchema: "prj",
                        principalTable: "Stage",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StageJobs_JobId",
                schema: "prj",
                table: "StageJobs",
                column: "JobId",
                unique: true,
                filter: "[JobId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_StageJobs_StageId",
                schema: "prj",
                table: "StageJobs",
                column: "StageId");
        }
    }
}

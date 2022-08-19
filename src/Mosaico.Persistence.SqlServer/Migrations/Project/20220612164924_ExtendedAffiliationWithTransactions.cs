using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class ExtendedAffiliationWithTransactions : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partners_Projects_ProjectId",
                schema: "prj",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "RewardClaimTransactionHash",
                schema: "prj",
                table: "PartnerTransactions");

            migrationBuilder.DropColumn(
                name: "RewardClaimed",
                schema: "prj",
                table: "PartnerTransactions");

            migrationBuilder.DropColumn(
                name: "RewardClaimedAt",
                schema: "prj",
                table: "PartnerTransactions");

            migrationBuilder.DropColumn(
                name: "TransactionHash",
                schema: "prj",
                table: "PartnerTransactions");

            migrationBuilder.DropColumn(
                name: "Email",
                schema: "prj",
                table: "Partners");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "prj",
                table: "PartnerTransactions",
                newName: "TransactionCorrelationId");

            migrationBuilder.RenameColumn(
                name: "RewardAmount",
                schema: "prj",
                table: "PartnerTransactions",
                newName: "PurchasedTokens");

            migrationBuilder.RenameColumn(
                name: "PurchasedTokenAmount",
                schema: "prj",
                table: "PartnerTransactions",
                newName: "EstimatedReward");

            migrationBuilder.RenameColumn(
                name: "UserId",
                schema: "prj",
                table: "Partners",
                newName: "FailureReason");

            migrationBuilder.RenameColumn(
                name: "TokensFee",
                schema: "prj",
                table: "Partners",
                newName: "RewardPercentage");

            migrationBuilder.RenameColumn(
                name: "ProjectId",
                schema: "prj",
                table: "Partners",
                newName: "UserAffiliationId");

            migrationBuilder.RenameIndex(
                name: "IX_Partners_ProjectId",
                schema: "prj",
                table: "Partners",
                newName: "IX_Partners_UserAffiliationId");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectAffiliationId",
                schema: "prj",
                table: "Projects",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PaymentStatus",
                schema: "prj",
                table: "Partners",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectAffiliationId",
                schema: "prj",
                table: "Partners",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateTable(
                name: "ProjectAffiliations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    RewardPercentage = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    RewardPool = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartsAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectAffiliations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectAffiliations_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserAffiliations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccessCode = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserAffiliations", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Partners_ProjectAffiliationId",
                schema: "prj",
                table: "Partners",
                column: "ProjectAffiliationId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectAffiliations_ProjectId",
                schema: "prj",
                table: "ProjectAffiliations",
                column: "ProjectId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_ProjectAffiliations_ProjectAffiliationId",
                schema: "prj",
                table: "Partners",
                column: "ProjectAffiliationId",
                principalSchema: "prj",
                principalTable: "ProjectAffiliations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_UserAffiliations_UserAffiliationId",
                schema: "prj",
                table: "Partners",
                column: "UserAffiliationId",
                principalSchema: "prj",
                principalTable: "UserAffiliations",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Partners_ProjectAffiliations_ProjectAffiliationId",
                schema: "prj",
                table: "Partners");

            migrationBuilder.DropForeignKey(
                name: "FK_Partners_UserAffiliations_UserAffiliationId",
                schema: "prj",
                table: "Partners");

            migrationBuilder.DropTable(
                name: "ProjectAffiliations",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "UserAffiliations",
                schema: "prj");

            migrationBuilder.DropIndex(
                name: "IX_Partners_ProjectAffiliationId",
                schema: "prj",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "ProjectAffiliationId",
                schema: "prj",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "PaymentStatus",
                schema: "prj",
                table: "Partners");

            migrationBuilder.DropColumn(
                name: "ProjectAffiliationId",
                schema: "prj",
                table: "Partners");

            migrationBuilder.RenameColumn(
                name: "TransactionCorrelationId",
                schema: "prj",
                table: "PartnerTransactions",
                newName: "UserId");

            migrationBuilder.RenameColumn(
                name: "PurchasedTokens",
                schema: "prj",
                table: "PartnerTransactions",
                newName: "RewardAmount");

            migrationBuilder.RenameColumn(
                name: "EstimatedReward",
                schema: "prj",
                table: "PartnerTransactions",
                newName: "PurchasedTokenAmount");

            migrationBuilder.RenameColumn(
                name: "UserAffiliationId",
                schema: "prj",
                table: "Partners",
                newName: "ProjectId");

            migrationBuilder.RenameColumn(
                name: "RewardPercentage",
                schema: "prj",
                table: "Partners",
                newName: "TokensFee");

            migrationBuilder.RenameColumn(
                name: "FailureReason",
                schema: "prj",
                table: "Partners",
                newName: "UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Partners_UserAffiliationId",
                schema: "prj",
                table: "Partners",
                newName: "IX_Partners_ProjectId");

            migrationBuilder.AddColumn<string>(
                name: "RewardClaimTransactionHash",
                schema: "prj",
                table: "PartnerTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "RewardClaimed",
                schema: "prj",
                table: "PartnerTransactions",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "RewardClaimedAt",
                schema: "prj",
                table: "PartnerTransactions",
                type: "datetimeoffset",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "TransactionHash",
                schema: "prj",
                table: "PartnerTransactions",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Email",
                schema: "prj",
                table: "Partners",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Partners_Projects_ProjectId",
                schema: "prj",
                table: "Partners",
                column: "ProjectId",
                principalSchema: "prj",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}

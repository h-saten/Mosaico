using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class ExtendedTransactionsWithFees : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FeeInUSD",
                schema: "wlt",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "FeePercentage",
                schema: "wlt",
                table: "Transactions",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "GasFee",
                schema: "wlt",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MosaicoFee",
                schema: "wlt",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "MosaicoFeeInUSD",
                schema: "wlt",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "TokenPrice",
                schema: "wlt",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "FeeToProjects",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    FeePercentage = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FeeToProjects", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FeeToProjects",
                schema: "wlt");

            migrationBuilder.DropColumn(
                name: "FeeInUSD",
                schema: "wlt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "FeePercentage",
                schema: "wlt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "GasFee",
                schema: "wlt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MosaicoFee",
                schema: "wlt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "MosaicoFeeInUSD",
                schema: "wlt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "TokenPrice",
                schema: "wlt",
                table: "Transactions");
        }
    }
}

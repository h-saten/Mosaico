using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedAgentsAndOperations : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "ExchangeRate",
                schema: "wlt",
                table: "Transactions",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SalesAgentId",
                schema: "wlt",
                table: "Transactions",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Operations",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TransactionId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Type = table.Column<int>(type: "int", nullable: false),
                    State = table.Column<int>(type: "int", nullable: false),
                    FinishedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TransactionHash = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    GasUsed = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    PayedNativeCurrency = table.Column<decimal>(type: "decimal(18,2)", nullable: true),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AccountAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ExtraData = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    FailureReason = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Operations", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "SalesAgents",
                schema: "wlt",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Company = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SalesAgents", x => x.Id);
                });

            migrationBuilder.InsertData(
                schema: "wlt",
                table: "TransactionStatuses",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[] { new Guid("8d50f357-0853-4aef-89c9-cebfe4c2c2e2"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "EXPIRED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Expired", 0L });

            migrationBuilder.CreateIndex(
                name: "IX_Transactions_SalesAgentId",
                schema: "wlt",
                table: "Transactions",
                column: "SalesAgentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Transactions_SalesAgents_SalesAgentId",
                schema: "wlt",
                table: "Transactions",
                column: "SalesAgentId",
                principalSchema: "wlt",
                principalTable: "SalesAgents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Transactions_SalesAgents_SalesAgentId",
                schema: "wlt",
                table: "Transactions");

            migrationBuilder.DropTable(
                name: "Operations",
                schema: "wlt");

            migrationBuilder.DropTable(
                name: "SalesAgents",
                schema: "wlt");

            migrationBuilder.DropIndex(
                name: "IX_Transactions_SalesAgentId",
                schema: "wlt",
                table: "Transactions");

            migrationBuilder.DeleteData(
                schema: "wlt",
                table: "TransactionStatuses",
                keyColumn: "Id",
                keyValue: new Guid("8d50f357-0853-4aef-89c9-cebfe4c2c2e2"));

            migrationBuilder.DropColumn(
                name: "ExchangeRate",
                schema: "wlt",
                table: "Transactions");

            migrationBuilder.DropColumn(
                name: "SalesAgentId",
                schema: "wlt",
                table: "Transactions");
        }
    }
}

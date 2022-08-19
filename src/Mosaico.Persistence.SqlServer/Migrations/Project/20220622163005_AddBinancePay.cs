using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class AddBinancePay : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "prj",
                table: "PaymentMethods",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[] { new Guid("614005f3-b356-45e8-96a2-b6e2a880240a"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "BINANCE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Binance", 0L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "prj",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: new Guid("614005f3-b356-45e8-96a2-b6e2a880240a"));
        }
    }
}

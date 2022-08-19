using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Wallet
{
    public partial class AddedRefundedTransactionStatus : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "wlt",
                table: "TransactionStatuses",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[] { new Guid("fa2f96aa-e42e-443e-a522-643c94b4f510"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "REFUNDED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Refunded", 0L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "wlt",
                table: "TransactionStatuses",
                keyColumn: "Id",
                keyValue: new Guid("fa2f96aa-e42e-443e-a522-643c94b4f510"));
        }
    }
}

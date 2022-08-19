using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class AddedBankTransferPaymentMethod : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                schema: "prj",
                table: "PaymentMethods",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[] { new Guid("0a26d7fc-3323-483f-b2f3-a30e033af7e2"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "BANK_TRANSFER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Bank Transfer", 0L });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                schema: "prj",
                table: "PaymentMethods",
                keyColumn: "Id",
                keyValue: new Guid("0a26d7fc-3323-483f-b2f3-a30e033af7e2"));
        }
    }
}

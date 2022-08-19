using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.ApplicationDb
{
    public partial class PhoneNumberCodeExpireDate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "ExpiresAt",
                schema: "dbo",
                table: "PhoneNumberConfirmationCodes",
                type: "datetimeoffset",
                nullable: false,
                defaultValue: new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExpiresAt",
                schema: "dbo",
                table: "PhoneNumberConfirmationCodes");
        }
    }
}

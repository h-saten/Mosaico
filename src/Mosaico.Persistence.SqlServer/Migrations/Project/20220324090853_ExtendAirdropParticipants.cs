using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class ExtendAirdropParticipants : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "TransactionHash",
                schema: "prj",
                table: "AirdropParticipants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                schema: "prj",
                table: "AirdropParticipants",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<DateTimeOffset>(
                name: "WithdrawnAt",
                schema: "prj",
                table: "AirdropParticipants",
                type: "datetimeoffset",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TransactionHash",
                schema: "prj",
                table: "AirdropParticipants");

            migrationBuilder.DropColumn(
                name: "UserId",
                schema: "prj",
                table: "AirdropParticipants");

            migrationBuilder.DropColumn(
                name: "WithdrawnAt",
                schema: "prj",
                table: "AirdropParticipants");
        }
    }
}

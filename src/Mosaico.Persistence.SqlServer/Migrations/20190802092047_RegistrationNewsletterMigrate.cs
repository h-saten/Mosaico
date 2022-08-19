using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations
{
    public partial class RegistrationNewsletterMigrate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "NewsletterDataProcessingAgree",
                table: "AspNetUsers",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "NewsletterDataProcessingAgreedDate",
                table: "AspNetUsers",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "NewsletterDataProcessingAgree",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "NewsletterDataProcessingAgreedDate",
                table: "AspNetUsers");
        }
    }
}

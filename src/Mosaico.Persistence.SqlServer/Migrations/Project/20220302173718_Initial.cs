using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace Mosaico.Persistence.SqlServer.Migrations.Project
{
    public partial class Initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "prj");

            migrationBuilder.CreateTable(
                name: "Abouts",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Abouts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTemplates",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TemplateVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    IsEnabled = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTemplates", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "DocumentTypes",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumentTypes", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "PaymentMethods",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PaymentMethods", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectRoles",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectRoles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ProjectStatuses",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "StageStatuses",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Key = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StageStatuses", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "AboutContent",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutContent_Abouts_AboutId",
                        column: x => x.AboutId,
                        principalSchema: "prj",
                        principalTable: "Abouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Pages",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AboutId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    ShortDescriptionId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Pages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Pages_Abouts_AboutId",
                        column: x => x.AboutId,
                        principalSchema: "prj",
                        principalTable: "Abouts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "AboutContentTranslations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AboutContentTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AboutContentTranslations_AboutContent_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "prj",
                        principalTable: "AboutContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Faqs",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    TitleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ContentId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Faqs", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Faqs_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "prj",
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentPackages",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TokenAmount = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentPackages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentPackages_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "prj",
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageCovers",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PrimaryColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SecondaryColor = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageCovers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageCovers_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "prj",
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageMembers",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Position = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Facebook = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Twitter = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    LinkedIn = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    PhotoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageMembers_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "prj",
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageSocialMediaLinks",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Order = table.Column<int>(type: "int", nullable: false),
                    IsHidden = table.Column<bool>(type: "bit", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageSocialMediaLinks", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageSocialMediaLinks_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "prj",
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Projects",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Description = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Slug = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    SlugInvariant = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    TitleInvariant = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    LogoUrl = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    TokenId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CompanyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    IsVisible = table.Column<bool>(type: "bit", nullable: false),
                    CrowdsaleId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    LegacyId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Projects", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Projects_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "prj",
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Projects_ProjectStatuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "prj",
                        principalTable: "ProjectStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShortDescription",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortDescription", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortDescription_Pages_PageId",
                        column: x => x.PageId,
                        principalSchema: "prj",
                        principalTable: "Pages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaqContent",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FaqId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqContent", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqContent_Faqs_FaqId",
                        column: x => x.FaqId,
                        principalSchema: "prj",
                        principalTable: "Faqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaqTitle",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    FaqId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    Title = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Key = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqTitle", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqTitle_Faqs_FaqId",
                        column: x => x.FaqId,
                        principalSchema: "prj",
                        principalTable: "Faqs",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "InvestmentPackageTranslations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Benefits = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InvestmentPackageTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_InvestmentPackageTranslations_InvestmentPackages_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "prj",
                        principalTable: "InvestmentPackages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PageCoverTranslations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PageCoverTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PageCoverTranslations_PageCovers_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "prj",
                        principalTable: "PageCovers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SocialMediaLinkTranslations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SocialMediaLinkTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SocialMediaLinkTranslations_PageSocialMediaLinks_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "prj",
                        principalTable: "PageSocialMediaLinks",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Articles",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VisibleText = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    AuthorPhoto = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CoverPicture = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Date = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    Link = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    AuthorName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Hidden = table.Column<bool>(type: "bit", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Articles", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Articles_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Crowdsales",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Network = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    OwnerAddress = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ContractVersion = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    HardCap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SoftCap = table.Column<decimal>(type: "decimal(18,2)", nullable: false),
                    SupportedStableCoins = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Crowdsales", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Crowdsales_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Documents",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    TypeId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Url = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Documents", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Documents_DocumentTypes_TypeId",
                        column: x => x.TypeId,
                        principalSchema: "prj",
                        principalTable: "DocumentTypes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Documents_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectMembers",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    RoleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    IsInvitationSent = table.Column<bool>(type: "bit", nullable: false),
                    ExpiresAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    AcceptedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    IsAccepted = table.Column<bool>(type: "bit", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    AuthorizationCode = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectMembers", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_ProjectRoles_RoleId",
                        column: x => x.RoleId,
                        principalSchema: "prj",
                        principalTable: "ProjectRoles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectMembers_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectNewsletterSubscriptions",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Email = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    NormalizedEmail = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    SubscribedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Code = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectNewsletterSubscriptions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProjectNewsletterSubscriptions_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ProjectPaymentMethod",
                schema: "prj",
                columns: table => new
                {
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PaymentMethodId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProjectPaymentMethod", x => new { x.ProjectId, x.PaymentMethodId });
                    table.ForeignKey(
                        name: "FK_ProjectPaymentMethod_PaymentMethods_PaymentMethodId",
                        column: x => x.PaymentMethodId,
                        principalSchema: "prj",
                        principalTable: "PaymentMethods",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ProjectPaymentMethod_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Stage",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Type = table.Column<int>(type: "int", nullable: false),
                    ProjectId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    StartDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    EndDate = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: true),
                    TokensSupply = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: false),
                    TokenPrice = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: false),
                    TokenPriceNativeCurrency = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: false),
                    MinimumPurchase = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: false),
                    MaximumPurchase = table.Column<decimal>(type: "decimal(36,18)", precision: 36, scale: 18, nullable: false),
                    StatusId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    VestingId = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Order = table.Column<int>(type: "int", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Stage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Stage_Projects_ProjectId",
                        column: x => x.ProjectId,
                        principalSchema: "prj",
                        principalTable: "Projects",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Stage_StageStatuses_StatusId",
                        column: x => x.StatusId,
                        principalSchema: "prj",
                        principalTable: "StageStatuses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ShortDescriptionTranslations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ShortDescriptionTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ShortDescriptionTranslations_ShortDescription_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "prj",
                        principalTable: "ShortDescription",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaqContentTranslations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqContentTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqContentTranslations_FaqContent_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "prj",
                        principalTable: "FaqContent",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "FaqTitleTranslations",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Version = table.Column<long>(type: "bigint", nullable: false),
                    EntityId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Language = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    Value = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FaqTitleTranslations", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FaqTitleTranslations_FaqTitle_EntityId",
                        column: x => x.EntityId,
                        principalSchema: "prj",
                        principalTable: "FaqTitle",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "StageJobs",
                schema: "prj",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobId = table.Column<string>(type: "nvarchar(450)", nullable: true),
                    StageId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    JobName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CreatedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    ModifiedAt = table.Column<DateTimeOffset>(type: "datetimeoffset", nullable: false),
                    CreatedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    ModifiedById = table.Column<string>(type: "nvarchar(max)", nullable: true),
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

            migrationBuilder.InsertData(
                schema: "prj",
                table: "DocumentTemplates",
                columns: new[] { "Id", "Content", "CreatedAt", "CreatedById", "IsEnabled", "Key", "Language", "ModifiedAt", "ModifiedById", "TemplateVersion", "Version" },
                values: new object[,]
                {
                    { new Guid("39e51fcf-afa2-48f6-9a65-ff03f63bf939"), "Lorem ipsum dolor sit amet, consectetur adipiscing elit. Suspendisse non lectus nec quam cursus eleifend pulvinar non ligula. Maecenas neque velit, sagittis sit amet tincidunt sed, auctor vel nulla. Vestibulum efficitur tempus quam, dignissim consequat dui viverra eget. Pellentesque ullamcorper, nunc a luctus tincidunt, orci urna venenatis velit, nec accumsan odio massa a turpis. Curabitur nec iaculis arcu, sed lobortis lectus. Sed scelerisque diam sed erat scelerisque elementum. Ut pretium nunc justo, vitae ultricies justo commodo vel.", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, "WHITE_PAPER", "en", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "0.1", 0L },
                    { new Guid("d8a8944c-7a06-46c5-adc4-ae38cfae0da7"), "Cras varius mattis laoreet. Morbi maximus placerat diam, in vulputate massa maximus non. In vitae venenatis risus, vitae luctus lacus. Vestibulum est sapien, interdum sed metus at, blandit volutpat odio. Curabitur nec nisi quam. Nullam sed dolor in nisi maximus sagittis. Quisque volutpat dignissim vehicula. Praesent tortor nibh, hendrerit sed laoreet sed, convallis ac neque. Ut vel erat eu arcu rutrum tempor vel tempor est. Quisque mollis nunc vitae fringilla ultricies. Praesent id justo orci. Duis sit amet dolor quis leo lacinia commodo. Ut ut pretium erat.", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, "WHITE_PAPER", "pl", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "0.1", 0L },
                    { new Guid("29a30233-58ea-47c1-a5a4-f979e88c7bad"), "Ut quis neque nec odio porta ultrices a vel magna. Pellentesque fringilla suscipit quam nec iaculis. Pellentesque scelerisque, sapien sed eleifend maximus, nisi augue commodo ante, eget finibus lorem purus vitae est. Vivamus euismod fermentum eros, sed vestibulum justo tempor vel. Pellentesque ac fringilla tortor. Nam urna enim, mattis molestie tortor sit amet, eleifend commodo erat. Sed tempor luctus enim, et efficitur mi cursus sit amet. Aliquam scelerisque, est ac posuere viverra, enim velit eleifend justo, vel tristique leo lorem eget enim. Curabitur elit mi, laoreet a mi sed, luctus pellentesque augue. Nunc tellus elit, pretium ac viverra sit amet, commodo vel magna. Donec vitae ultricies arcu.", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, "TERMS_AND_CONDITIONS", "en", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "0.1", 0L },
                    { new Guid("ec5edbdd-3f32-42d8-9a52-0e6a5753d0cd"), "Sed sollicitudin ornare sem, vitae pellentesque odio. Sed vel elit feugiat, ornare quam dictum, varius lectus. Suspendisse potenti. Pellentesque lobortis eros nec purus consequat tristique. Sed sodales gravida ex ac condimentum. Aenean sit amet neque vel mi sodales volutpat. Aliquam neque urna, pretium ut eros non, ullamcorper dictum nunc. Pellentesque semper, justo ut tincidunt tristique, diam metus posuere augue, sed finibus quam arcu id libero. Nulla vestibulum scelerisque purus id placerat. Integer feugiat viverra consectetur. Nulla sit amet auctor elit, eu tempus velit. Nulla vitae lacus aliquet, aliquet dolor et, vulputate dolor. Etiam arcu leo, lobortis quis massa consectetur, tincidunt accumsan velit.", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, "TERMS_AND_CONDITIONS", "pl", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "0.1", 0L },
                    { new Guid("69e52c4a-4207-475f-b8e2-551c687d213f"), "Pellentesque ut ipsum a est tincidunt ornare. In euismod ullamcorper venenatis. Nullam at justo nisl. Nunc sed fringilla mi, laoreet facilisis odio. Fusce in neque est. Cras lacus nunc, vulputate ac suscipit eget, volutpat at ante. Sed in justo tempor, venenatis magna a, suscipit orci. In sit amet condimentum elit. Mauris vitae nunc at arcu tincidunt tristique. In hac habitasse platea dictumst. In ac metus sed risus dignissim ullamcorper.", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, "PRIVACY_POLICY", "en", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "0.1", 0L },
                    { new Guid("6b2390d7-6c25-42de-83a8-b81b025ecc4e"), "Donec justo enim, euismod fringilla velit faucibus, commodo ornare libero. Integer mattis sem quis tellus hendrerit molestie non eget nisl. Nulla a pulvinar felis. Curabitur facilisis quam in ante auctor, nec euismod mi sagittis. Maecenas eget leo vitae nulla pulvinar ultrices. Mauris iaculis fringilla lacinia. Integer et semper mauris. Duis dapibus enim sit amet est efficitur condimentum. Vivamus varius efficitur sapien, eget semper arcu volutpat nec. Praesent efficitur accumsan orci id faucibus. Etiam convallis velit eu odio bibendum, non dignissim turpis gravida. Proin ultricies eros in diam iaculis, ac rutrum nulla sollicitudin. Fusce eu nisi tortor. Nulla hendrerit erat vitae mi efficitur, luctus pulvinar purus pharetra. Cras at imperdiet mauris. Duis sed porta neque.", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, true, "PRIVACY_POLICY", "pl", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "0.1", 0L }
                });

            migrationBuilder.InsertData(
                schema: "prj",
                table: "DocumentTypes",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Order", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("f604d78a-eba1-4f00-a59c-83e54e16686a"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "WHITE_PAPER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 1000, "Whitepaper", 0L },
                    { new Guid("0b669ab6-9c3f-4a44-97e4-38d45130a6c3"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "TERMS_AND_CONDITIONS", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 2000, "Terms And Conditions", 0L },
                    { new Guid("f527695b-73b2-405c-b877-45ba6fd47e26"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "PRIVACY_POLICY", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, 3000, "Privacy Policy", 0L }
                });

            migrationBuilder.InsertData(
                schema: "prj",
                table: "PaymentMethods",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("6cc3cf07-7e91-4077-b0a6-0b546b79a226"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "KANGA_EXCHANGE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Kanga Exchange", 0L },
                    { new Guid("051d6c04-551b-4f2f-a8b6-bcce1481306f"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "CREDIT_CARD", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Credit Card", 0L },
                    { new Guid("669ac898-d597-46c0-b9ee-1aaca19d6153"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "MOSAICO_WALLET", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Mosaico Wallet", 0L },
                    { new Guid("4b21fbdf-9d11-48e2-a770-4a0a29f5d693"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "METAMASK", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Metamask", 0L }
                });

            migrationBuilder.InsertData(
                schema: "prj",
                table: "ProjectRoles",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("f476fa5c-5483-4c88-8d82-280dc95ba424"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "OWNER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Owner", 0L },
                    { new Guid("10e1b662-8d5b-4fb8-9632-86cde1e7f5ec"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "MEMBER", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Member", 0L }
                });

            migrationBuilder.InsertData(
                schema: "prj",
                table: "ProjectStatuses",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("74246a47-a93d-4713-b8c7-4f51263947ce"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "NEW", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "New", 0L },
                    { new Guid("5730abcf-134b-4116-b186-0e1f54c1d1c6"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "UNDER_REVIEW", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Under review", 0L },
                    { new Guid("9aa58972-d2c8-467d-a162-7ea773e5aded"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "APPROVED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Approved", 0L },
                    { new Guid("7529ec5c-5351-44f9-bea6-e89d27a3bd23"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "DECLINED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Declined", 0L },
                    { new Guid("6d150791-925f-4c7e-8f9a-87d31e3aa061"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "IN_PROGRESS", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "In Progress", 0L },
                    { new Guid("fc4982ea-5b11-4a75-a2d4-4d192ba42848"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "CLOSED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Closed", 0L }
                });

            migrationBuilder.InsertData(
                schema: "prj",
                table: "StageStatuses",
                columns: new[] { "Id", "CreatedAt", "CreatedById", "Key", "ModifiedAt", "ModifiedById", "Title", "Version" },
                values: new object[,]
                {
                    { new Guid("b13cf50e-6e69-4c9d-a928-dfd340854bf9"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "ACTIVE", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Active", 0L },
                    { new Guid("71fa9290-23e6-49e4-8bf9-b0f1083793c7"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "PENDING", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Pending", 0L },
                    { new Guid("a54554e8-e98c-406a-abaf-e383291f029f"), new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "CLOSED", new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)), null, "Closed", 0L }
                });

            migrationBuilder.CreateIndex(
                name: "IX_AboutContent_AboutId",
                schema: "prj",
                table: "AboutContent",
                column: "AboutId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_AboutContentTranslations_EntityId",
                schema: "prj",
                table: "AboutContentTranslations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Articles_ProjectId",
                schema: "prj",
                table: "Articles",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Crowdsales_ProjectId",
                schema: "prj",
                table: "Crowdsales",
                column: "ProjectId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Documents_ProjectId",
                schema: "prj",
                table: "Documents",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Documents_TypeId",
                schema: "prj",
                table: "Documents",
                column: "TypeId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqContent_FaqId",
                schema: "prj",
                table: "FaqContent",
                column: "FaqId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FaqContentTranslations_EntityId",
                schema: "prj",
                table: "FaqContentTranslations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Faqs_PageId",
                schema: "prj",
                table: "Faqs",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_FaqTitle_FaqId",
                schema: "prj",
                table: "FaqTitle",
                column: "FaqId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_FaqTitleTranslations_EntityId",
                schema: "prj",
                table: "FaqTitleTranslations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentPackages_PageId",
                schema: "prj",
                table: "InvestmentPackages",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_InvestmentPackageTranslations_EntityId",
                schema: "prj",
                table: "InvestmentPackageTranslations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PageCovers_PageId",
                schema: "prj",
                table: "PageCovers",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PageCoverTranslations_EntityId",
                schema: "prj",
                table: "PageCoverTranslations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_PageMembers_PageId",
                schema: "prj",
                table: "PageMembers",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_Pages_AboutId",
                schema: "prj",
                table: "Pages",
                column: "AboutId",
                unique: true,
                filter: "[AboutId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_PageSocialMediaLinks_PageId",
                schema: "prj",
                table: "PageSocialMediaLinks",
                column: "PageId");

            migrationBuilder.CreateIndex(
                name: "IX_PaymentMethods_Key",
                schema: "prj",
                table: "PaymentMethods",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_AuthorizationCode",
                schema: "prj",
                table: "ProjectMembers",
                column: "AuthorizationCode",
                unique: true,
                filter: "[AuthorizationCode] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_Email_ProjectId",
                schema: "prj",
                table: "ProjectMembers",
                columns: new[] { "Email", "ProjectId" },
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_ProjectId",
                schema: "prj",
                table: "ProjectMembers",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectMembers_RoleId",
                schema: "prj",
                table: "ProjectMembers",
                column: "RoleId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNewsletterSubscriptions_Email_ProjectId",
                schema: "prj",
                table: "ProjectNewsletterSubscriptions",
                columns: new[] { "Email", "ProjectId" },
                unique: true,
                filter: "[Email] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectNewsletterSubscriptions_ProjectId",
                schema: "prj",
                table: "ProjectNewsletterSubscriptions",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectPaymentMethod_PaymentMethodId",
                schema: "prj",
                table: "ProjectPaymentMethod",
                column: "PaymentMethodId");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectRoles_Key",
                schema: "prj",
                table: "ProjectRoles",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_PageId",
                schema: "prj",
                table: "Projects",
                column: "PageId",
                unique: true,
                filter: "[PageId] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_Slug",
                schema: "prj",
                table: "Projects",
                column: "Slug",
                unique: true,
                filter: "[Slug] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_SlugInvariant",
                schema: "prj",
                table: "Projects",
                column: "SlugInvariant",
                unique: true,
                filter: "[SlugInvariant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_StatusId",
                schema: "prj",
                table: "Projects",
                column: "StatusId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_TitleInvariant",
                schema: "prj",
                table: "Projects",
                column: "TitleInvariant",
                unique: true,
                filter: "[TitleInvariant] IS NOT NULL");

            migrationBuilder.CreateIndex(
                name: "IX_ProjectStatuses_Key",
                schema: "prj",
                table: "ProjectStatuses",
                column: "Key",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortDescription_PageId",
                schema: "prj",
                table: "ShortDescription",
                column: "PageId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_ShortDescriptionTranslations_EntityId",
                schema: "prj",
                table: "ShortDescriptionTranslations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_SocialMediaLinkTranslations_EntityId",
                schema: "prj",
                table: "SocialMediaLinkTranslations",
                column: "EntityId");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_ProjectId",
                schema: "prj",
                table: "Stage",
                column: "ProjectId");

            migrationBuilder.CreateIndex(
                name: "IX_Stage_StatusId",
                schema: "prj",
                table: "Stage",
                column: "StatusId");

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

            migrationBuilder.CreateIndex(
                name: "IX_StageStatuses_Key",
                schema: "prj",
                table: "StageStatuses",
                column: "Key",
                unique: true,
                filter: "[Key] IS NOT NULL");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "AboutContentTranslations",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "Articles",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "Crowdsales",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "Documents",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "DocumentTemplates",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "FaqContentTranslations",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "FaqTitleTranslations",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "InvestmentPackageTranslations",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "PageCoverTranslations",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "PageMembers",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "ProjectMembers",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "ProjectNewsletterSubscriptions",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "ProjectPaymentMethod",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "ShortDescriptionTranslations",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "SocialMediaLinkTranslations",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "StageJobs",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "AboutContent",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "DocumentTypes",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "FaqContent",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "FaqTitle",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "InvestmentPackages",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "PageCovers",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "ProjectRoles",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "PaymentMethods",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "ShortDescription",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "PageSocialMediaLinks",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "Stage",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "Faqs",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "Projects",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "StageStatuses",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "Pages",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "ProjectStatuses",
                schema: "prj");

            migrationBuilder.DropTable(
                name: "Abouts",
                schema: "prj");
        }
    }
}

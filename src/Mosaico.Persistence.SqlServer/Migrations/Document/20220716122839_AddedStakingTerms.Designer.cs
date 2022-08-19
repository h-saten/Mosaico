﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mosaico.Persistence.SqlServer.Contexts.DocumentContext;

namespace Mosaico.Persistence.SqlServer.Migrations.Document
{
    [DbContext(typeof(DocumentContext))]
    [Migration("20220716122839_AddedStakingTerms")]
    partial class AddedStakingTerms
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.DocumentBase", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Discriminator")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Documents", "doc");

                    b.HasDiscriminator<string>("Discriminator").HasValue("DocumentBase");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.DocumentContent", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("DocumentAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid?>("DocumentBaseId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("DocumentId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("FileId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("DocumentBaseId");

                    b.HasIndex("DocumentId", "Language")
                        .IsUnique();

                    b.ToTable("DocumentContents", "doc");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.ArticleCover", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ArticleCover_ArticleId");

                    b.HasIndex("ArticleId")
                        .IsUnique()
                        .HasFilter("[ArticleCover_ArticleId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("ArticleCover");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.ArticlePhoto", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("ArticleId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("ArticleId")
                        .IsUnique()
                        .HasFilter("[ArticleId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("ArticlePhoto");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.CompanyDocument", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("CompanyDocument_CompanyId");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("bit");

                    b.HasDiscriminator().HasValue("CompanyDocument");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.CompanyLogo", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("CompanyId")
                        .IsUnique()
                        .HasFilter("[CompanyId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("CompanyLogo");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.InvestmentPackageLogo", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("InvestmentPackageId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("PageId", "InvestmentPackageId")
                        .IsUnique()
                        .HasFilter("[PageId] IS NOT NULL AND [InvestmentPackageId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("InvestmentPackageLogo");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.PageCover", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("PageId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("PageCover_PageId");

                    b.HasIndex("PageId")
                        .IsUnique()
                        .HasFilter("[PageCover_PageId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("PageCover");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.ProjectDocument", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<bool>("IsMandatory")
                        .HasColumnType("bit")
                        .HasColumnName("ProjectDocument_IsMandatory");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier");

                    b.HasDiscriminator().HasValue("ProjectDocument");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.ProjectLogo", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("ProjectId")
                        .HasColumnType("uniqueidentifier")
                        .HasColumnName("ProjectLogo_ProjectId");

                    b.HasIndex("ProjectId")
                        .IsUnique()
                        .HasFilter("[ProjectLogo_ProjectId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("ProjectLogo");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.StakingTermsDocument", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("StakingPairId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("StakingPairId")
                        .IsUnique()
                        .HasFilter("[StakingPairId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("StakingTermsDocument");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.TokenLogo", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("TokenId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("TokenId")
                        .IsUnique()
                        .HasFilter("[TokenId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("TokenLogo");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.UserPhoto", b =>
                {
                    b.HasBaseType("Mosaico.Domain.DocumentManagement.Entities.DocumentBase");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasIndex("UserId")
                        .IsUnique()
                        .HasFilter("[UserId] IS NOT NULL");

                    b.HasDiscriminator().HasValue("UserPhoto");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.DocumentContent", b =>
                {
                    b.HasOne("Mosaico.Domain.DocumentManagement.Entities.DocumentBase", null)
                        .WithMany("Contents")
                        .HasForeignKey("DocumentBaseId");

                    b.HasOne("Mosaico.Domain.DocumentManagement.Entities.DocumentBase", "Document")
                        .WithMany()
                        .HasForeignKey("DocumentId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Document");
                });

            modelBuilder.Entity("Mosaico.Domain.DocumentManagement.Entities.DocumentBase", b =>
                {
                    b.Navigation("Contents");
                });
#pragma warning restore 612, 618
        }
    }
}

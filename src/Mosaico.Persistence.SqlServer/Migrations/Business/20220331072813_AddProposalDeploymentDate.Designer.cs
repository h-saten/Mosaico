﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mosaico.Persistence.SqlServer.Contexts.BusinessContext;

namespace Mosaico.Persistence.SqlServer.Migrations.Business
{
    [DbContext(typeof(BusinessContext))]
    [Migration("20220331072813_AddProposalDeploymentDate")]
    partial class AddProposalDeploymentDate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("bsn")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Company", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("ApprovedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ApprovedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyDescription")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CompanyName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ContractAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Country")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Industry")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("InitialVotingDelay")
                        .HasColumnType("int");

                    b.Property<int>("InitialVotingPeriod")
                        .HasColumnType("int");

                    b.Property<bool>("IsApproved")
                        .HasColumnType("bit");

                    b.Property<bool>("IsVotingEnabled")
                        .HasColumnType("bit");

                    b.Property<string>("LegacyId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LogoUrl")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Network")
                        .HasColumnType("nvarchar(max)");

                    b.Property<bool>("OnlyOwnerProposals")
                        .HasColumnType("bit");

                    b.Property<string>("PhoneNumber")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PostalCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Quorum")
                        .HasColumnType("bigint");

                    b.Property<string>("Region")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Size")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Slug")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("Street")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("VATId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.Property<string>("Website")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("Slug")
                        .IsUnique()
                        .HasFilter("[Slug] IS NOT NULL");

                    b.ToTable("Companies", "bsn");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.CompanySubscriber", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Code")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(450)");

                    b.Property<string>("EmailNormalized")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("Email", "CompanyId")
                        .IsUnique()
                        .HasFilter("[Email] IS NOT NULL");

                    b.ToTable("CompanySubscribers", "bsn");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Document", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Content")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Language")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Url")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Documents", "bsn");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Proposal", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedByAddress")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("DeployedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Description")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("EndsAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Network")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("ProposalId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("QuorumThreshold")
                        .HasColumnType("int");

                    b.Property<DateTimeOffset>("StartsAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TokenId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("Proposals", "bsn");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Shareholder", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("FullName")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("Share")
                        .HasColumnType("int");

                    b.Property<Guid>("VerificationId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("VerificationId");

                    b.ToTable("Shareholder", "bsn");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.SocialMediaLinks", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Facebook")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Instagram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("LinkedIn")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Medium")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Telegram")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Twitter")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.Property<string>("Youtube")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.ToTable("SocialMediaLinks");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.TeamMember", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset?>("AcceptedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("AuthorizationCode")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Email")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset?>("ExpiresAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<bool>("IsAccepted")
                        .HasColumnType("bit");

                    b.Property<bool>("IsInvitationSent")
                        .HasColumnType("bit");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("TeamMemberRoleId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("UserId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("CompanyId");

                    b.HasIndex("TeamMemberRoleId");

                    b.ToTable("TeamMember", "bsn");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.TeamMemberRole", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Key")
                        .IsRequired()
                        .HasColumnType("nvarchar(450)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Title")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.HasIndex("Key")
                        .IsUnique();

                    b.ToTable("TeamMemberRole", "bsn");

                    b.HasData(
                        new
                        {
                            Id = new Guid("71fa9290-23e6-49e4-8bf9-b0f1083793c8"),
                            CreatedAt = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Key = "OWNER",
                            ModifiedAt = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Title = "Owner",
                            Version = 0L
                        },
                        new
                        {
                            Id = new Guid("b13cf50e-6e69-4c9d-a928-dfd340854bf7"),
                            CreatedAt = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Key = "MEMBER",
                            ModifiedAt = new DateTimeOffset(new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new TimeSpan(0, 0, 0, 0, 0)),
                            Title = "Member",
                            Version = 0L
                        });
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Verification", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyAddressUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("CompanyId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("CompanyRegistrationUrl")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.HasKey("Id");

                    b.ToTable("Verification", "bsn");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Vote", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<DateTimeOffset>("CreatedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("CreatedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("ModifiedAt")
                        .HasColumnType("datetimeoffset");

                    b.Property<string>("ModifiedById")
                        .HasColumnType("nvarchar(max)");

                    b.Property<Guid>("ProposalId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<int>("Result")
                        .HasColumnType("int");

                    b.Property<decimal>("Tokens")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("TransactionHash")
                        .HasColumnType("nvarchar(max)");

                    b.Property<long>("Version")
                        .HasColumnType("bigint");

                    b.Property<string>("VotedByAddress")
                        .HasColumnType("nvarchar(450)");

                    b.HasKey("Id");

                    b.HasIndex("ProposalId", "VotedByAddress")
                        .IsUnique()
                        .HasFilter("[VotedByAddress] IS NOT NULL");

                    b.ToTable("Votes", "bsn");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.CompanySubscriber", b =>
                {
                    b.HasOne("Mosaico.Domain.BusinessManagement.Entities.Company", "Company")
                        .WithMany("CompanySubscribers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Document", b =>
                {
                    b.HasOne("Mosaico.Domain.BusinessManagement.Entities.Company", "Company")
                        .WithMany("Documents")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Proposal", b =>
                {
                    b.HasOne("Mosaico.Domain.BusinessManagement.Entities.Company", "Company")
                        .WithMany("Proposals")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Shareholder", b =>
                {
                    b.HasOne("Mosaico.Domain.BusinessManagement.Entities.Verification", "Verification")
                        .WithMany("Shareholders")
                        .HasForeignKey("VerificationId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Verification");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.SocialMediaLinks", b =>
                {
                    b.HasOne("Mosaico.Domain.BusinessManagement.Entities.Company", "Company")
                        .WithMany()
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.TeamMember", b =>
                {
                    b.HasOne("Mosaico.Domain.BusinessManagement.Entities.Company", "Company")
                        .WithMany("TeamMembers")
                        .HasForeignKey("CompanyId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Mosaico.Domain.BusinessManagement.Entities.TeamMemberRole", "TeamMemberRole")
                        .WithMany()
                        .HasForeignKey("TeamMemberRoleId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Company");

                    b.Navigation("TeamMemberRole");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Vote", b =>
                {
                    b.HasOne("Mosaico.Domain.BusinessManagement.Entities.Proposal", "Proposal")
                        .WithMany("Votes")
                        .HasForeignKey("ProposalId")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("Proposal");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Company", b =>
                {
                    b.Navigation("CompanySubscribers");

                    b.Navigation("Documents");

                    b.Navigation("Proposals");

                    b.Navigation("TeamMembers");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Proposal", b =>
                {
                    b.Navigation("Votes");
                });

            modelBuilder.Entity("Mosaico.Domain.BusinessManagement.Entities.Verification", b =>
                {
                    b.Navigation("Shareholders");
                });
#pragma warning restore 612, 618
        }
    }
}

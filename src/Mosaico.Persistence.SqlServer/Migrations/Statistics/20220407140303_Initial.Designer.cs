﻿// <auto-generated />
using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Mosaico.Persistence.SqlServer.Contexts.StatisticsContext;

namespace Mosaico.Persistence.SqlServer.Migrations.Statistics
{
    [DbContext(typeof(StatisticsContext))]
    [Migration("20220407140303_Initial")]
    partial class Initial
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasDefaultSchema("stcs")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("ProductVersion", "5.0.12")
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("Mosaico.Domain.Statistics.Entities.PurchaseTransaction", b =>
                {
                    b.Property<Guid>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("uniqueidentifier");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTimeOffset>("Date")
                        .HasColumnType("datetimeoffset");

                    b.Property<decimal>("Payed")
                        .HasPrecision(36, 18)
                        .HasColumnType("decimal(36,18)");

                    b.Property<Guid>("TokenId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("TokensAmount")
                        .HasPrecision(36, 18)
                        .HasColumnType("decimal(36,18)");

                    b.Property<Guid>("TransactionId")
                        .HasColumnType("uniqueidentifier");

                    b.Property<decimal>("USDTAmount")
                        .HasPrecision(36, 18)
                        .HasColumnType("decimal(36,18)");

                    b.Property<Guid>("UserId")
                        .HasColumnType("uniqueidentifier");

                    b.HasKey("Id");

                    b.ToTable("PurchaseTransactions", "stcs");
                });
#pragma warning restore 612, 618
        }
    }
}
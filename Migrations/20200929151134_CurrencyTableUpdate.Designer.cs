﻿// <auto-generated />
using System;
using DigitalProductAPI.Contexts;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace DigitalProductAPI.Migrations
{
    [DbContext(typeof(SecuritiesContext))]
    [Migration("20200929151134_CurrencyTableUpdate")]
    partial class CurrencyTableUpdate
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("DigitalProductAPI.Entities.Bond", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AuthorizeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("AuthorizerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("AvailableVolume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("BidPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Consideration")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("FaceValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("InputDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsAuthorized")
                        .HasColumnType("bit");

                    b.Property<string>("MakerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaturityDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaturityValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("OfferPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SecurityID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Bonds");
                });

            modelBuilder.Entity("DigitalProductAPI.Entities.Currency", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<string>("CurrencyCode")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("CurrencyDescription")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateTime>("SetUpDate")
                        .HasColumnType("datetime2");

                    b.HasKey("Id");

                    b.ToTable("Currencies");
                });

            modelBuilder.Entity("DigitalProductAPI.Entities.TreasuryBill", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int")
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<DateTime>("AuthorizeDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("AuthorizerId")
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("AvailableVolume")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("BidPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Consideration")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("Currency")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("FaceValue")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime>("InputDate")
                        .HasColumnType("datetime2");

                    b.Property<bool>("IsAuthorized")
                        .HasColumnType("bit");

                    b.Property<string>("MakerId")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaturityDate")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("MaturityValue")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<decimal>("OfferPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<decimal>("Rate")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("SecurityID")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Status")
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("TreasuryBills");
                });
#pragma warning restore 612, 618
        }
    }
}

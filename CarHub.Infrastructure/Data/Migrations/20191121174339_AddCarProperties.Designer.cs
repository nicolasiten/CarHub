﻿// <auto-generated />
using System;
using CarHub.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace CarHub.Infrastructure.Data.Migrations
{
    [DbContext(typeof(ApplicationDbContext))]
    [Migration("20191121174339_AddCarProperties")]
    partial class AddCarProperties
    {
        protected override void BuildTargetModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "2.2.6-servicing-10079")
                .HasAnnotation("Relational:MaxIdentifierLength", 128)
                .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

            modelBuilder.Entity("CarHub.Core.Entities.Car", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int>("Kilometers");

                    b.Property<DateTime>("LotDate")
                        .HasColumnType("datetime2");

                    b.Property<string>("Make")
                        .IsRequired();

                    b.Property<string>("Model")
                        .IsRequired();

                    b.Property<DateTime>("PurchaseDate")
                        .HasColumnType("datetime2");

                    b.Property<decimal>("PurchasePrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<DateTime?>("SaleDate");

                    b.Property<decimal>("SellingPrice")
                        .HasColumnType("decimal(18,2)");

                    b.Property<bool>("ShowCase");

                    b.Property<int>("TransmissionType");

                    b.Property<string>("Trim")
                        .IsRequired();

                    b.Property<string>("Vin")
                        .IsRequired();

                    b.Property<int>("Year");

                    b.HasKey("Id");

                    b.ToTable("Car");
                });

            modelBuilder.Entity("CarHub.Core.Entities.Image", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CarFk");

                    b.Property<byte[]>("File")
                        .IsRequired()
                        .HasColumnType("image");

                    b.HasKey("Id");

                    b.HasIndex("CarFk");

                    b.ToTable("Image");
                });

            modelBuilder.Entity("CarHub.Core.Entities.Repair", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CarFk");

                    b.Property<decimal>("RepairCost")
                        .HasColumnType("decimal(18,2)");

                    b.Property<string>("RepairDescription")
                        .IsRequired();

                    b.HasKey("Id");

                    b.HasIndex("CarFk");

                    b.ToTable("Repair");
                });

            modelBuilder.Entity("CarHub.Core.Entities.Thumbnail", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasAnnotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn);

                    b.Property<int?>("CarFk");

                    b.Property<byte[]>("File")
                        .IsRequired()
                        .HasColumnType("image");

                    b.HasKey("Id");

                    b.HasIndex("CarFk")
                        .IsUnique()
                        .HasFilter("[CarFk] IS NOT NULL");

                    b.ToTable("Thumbnail");
                });

            modelBuilder.Entity("CarHub.Core.Entities.Image", b =>
                {
                    b.HasOne("CarHub.Core.Entities.Car", "Car")
                        .WithMany("Images")
                        .HasForeignKey("CarFk")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CarHub.Core.Entities.Repair", b =>
                {
                    b.HasOne("CarHub.Core.Entities.Car", "Car")
                        .WithMany("Repairs")
                        .HasForeignKey("CarFk")
                        .OnDelete(DeleteBehavior.Cascade);
                });

            modelBuilder.Entity("CarHub.Core.Entities.Thumbnail", b =>
                {
                    b.HasOne("CarHub.Core.Entities.Car", "Car")
                        .WithOne("ThumbnailImage")
                        .HasForeignKey("CarHub.Core.Entities.Thumbnail", "CarFk")
                        .OnDelete(DeleteBehavior.Cascade);
                });
#pragma warning restore 612, 618
        }
    }
}

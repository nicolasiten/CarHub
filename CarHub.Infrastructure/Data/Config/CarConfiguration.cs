using CarHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Data.Config
{
    public class CarConfiguration : IEntityTypeConfiguration<Car>
    {
        public void Configure(EntityTypeBuilder<Car> builder)
        {
            builder.HasKey(c => c.Id);

            builder.Property(c => c.Vin)
                .IsRequired();

            builder.Property(c => c.Make)
                .IsRequired();

            builder.Property(c => c.Model)
                .IsRequired();

            builder.Property(c => c.Trim)
                .IsRequired();

            builder.Property(c => c.PurchaseDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(c => c.LotDate)
                .IsRequired()
                .HasColumnType("datetime2");

            builder.Property(c => c.PurchasePrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.SellingPrice)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(c => c.SaleDate);

            builder.Property(c => c.ShowCase)
                .IsRequired();

            builder.ToTable("Car");
        }
    }
}

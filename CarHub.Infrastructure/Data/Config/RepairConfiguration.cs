using CarHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Data.Config
{
    public class RepairConfiguration : IEntityTypeConfiguration<Repair>
    {
        public void Configure(EntityTypeBuilder<Repair> builder)
        {
            builder.HasKey(r => r.Id);

            builder.Property(r => r.CarFk);

            builder.Property(r => r.RepairDescription)
                .IsRequired();

            builder.Property(r => r.RepairCost)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.HasIndex(r => r.CarFk);

            builder.HasOne(r => r.Car)
                .WithMany(c => c.Repairs)
                .HasForeignKey(r => r.CarFk)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Repair");
        }
    }
}

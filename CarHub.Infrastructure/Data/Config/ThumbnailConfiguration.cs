using CarHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Data.Config
{
    public class ThumbnailConfiguration : IEntityTypeConfiguration<Thumbnail>
    {
        public void Configure(EntityTypeBuilder<Thumbnail> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.CarFk);

            builder.Property(t => t.File)
                .HasColumnType("image")
                .IsRequired();

            builder.HasIndex(r => r.CarFk);

            builder.HasOne(t => t.Car)
                .WithOne(c => c.ThumbnailImage)
                .HasForeignKey<Thumbnail>(t => t.CarFk)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Thumbnail");
        }
    }
}

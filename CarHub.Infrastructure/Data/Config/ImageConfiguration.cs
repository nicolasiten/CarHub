using CarHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Data.Config
{
    public class ImageConfiguration : IEntityTypeConfiguration<Image>
    {
        public void Configure(EntityTypeBuilder<Image> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.CarFk);

            builder.Property(t => t.File)
                .HasColumnType("image")
                .IsRequired();

            builder.HasIndex(r => r.CarFk);

            builder.HasOne(t => t.Car)
                .WithMany(c => c.Images)
                .HasForeignKey(t => t.CarFk)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Image");
        }
    }
}

using CarHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Data.Config
{
    public class ThumbnailConfiguration : FileDataConfiguration<Thumbnail>
    {
        public override void Configure(EntityTypeBuilder<Thumbnail> builder)
        {
            base.Configure(builder);

            builder.HasOne(t => t.Car)
                .WithOne(c => c.ThumbnailImage)
                .HasForeignKey<Thumbnail>(t => t.CarFk)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Thumbnail");
        }
    }
}

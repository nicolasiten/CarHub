using CarHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Data.Config
{
    public class ImageConfiguration : FileDataConfiguration<Image>
    {
        public override void Configure(EntityTypeBuilder<Image> builder)
        {
            base.Configure(builder);

            builder.HasOne(t => t.Car)
                .WithMany(c => c.Images)
                .HasForeignKey(t => t.CarFk)
                .OnDelete(DeleteBehavior.Cascade);

            builder.ToTable("Image");
        }
    }
}

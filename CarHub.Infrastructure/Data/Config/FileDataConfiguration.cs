using CarHub.Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Text;

namespace CarHub.Infrastructure.Data.Config
{
    public abstract class FileDataConfiguration<T> : IEntityTypeConfiguration<T> where T : FileData
    {
        public virtual void Configure(EntityTypeBuilder<T> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.CarFk);

            builder.Property(t => t.File)
                .HasColumnType("image")
                .IsRequired();

            builder.Property(t => t.ImageType)
                .IsRequired();

            builder.HasIndex(r => r.CarFk);
        }
    }
}

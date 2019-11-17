using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using ProductCatalog.Models;

namespace ProductCatalog.Data.Maps
{
    public class ProductMap : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.ToTable("Product");
            builder.HasKey(p => p.Id);
            builder.Property(p => p.CreateDate).IsRequired();
            builder.Property(p => p.Description).IsRequired().HasMaxLength(1024).HasColumnType("varchar(1024)");
            builder.Property(p => p.Image).IsRequired().HasMaxLength(1024).HasColumnType("varchar(1024)");
            builder.Property(p => p.LastUpdateDate).IsRequired();
            builder.Property(p => p.Price).IsRequired().HasColumnType("money");
            builder.Property(p => p.Quantity).IsRequired();
            builder.Property(p => p.Title).IsRequired().HasMaxLength(120).HasColumnType("varchar(120)");        
            builder.HasOne(p => p.Category).WithMany(c => c.Products);

        }
    }
}
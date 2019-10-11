using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SS.GiftShop.Core;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Persistence.Configurations
{
    public class ProductConfiguration : IEntityTypeConfiguration<Product>
    {
        public void Configure(EntityTypeBuilder<Product> builder)
        {
            builder.Property(c => c.ProductName)
                .IsRequired()
                .HasMaxLength(AppConstants.StandardValueLength);

            builder.Property(c => c.Description)
                .HasMaxLength(AppConstants.MaxLength);

            builder.Property(c => c.Characteristics)
                .HasMaxLength(AppConstants.MaxLength);

            builder.Property(c => c.Price)
                .HasMaxLength(12);
                //.HasMaxLength(AppConstants.StandardValueLength);


        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SS.GiftShop.Core;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Persistence.Configurations
{
    public class OrderDetailConfiguration : IEntityTypeConfiguration<OrderDetail>
    {
        public void Configure(EntityTypeBuilder<OrderDetail> builder)
        {
            builder.Property(c => c.Quantity)
                .IsRequired();

            builder.Property(c => c.Total)
                .HasMaxLength(12);

            builder.Property(c => c.User)
                .HasMaxLength(AppConstants.StandardValueLength);
        }
    }
}

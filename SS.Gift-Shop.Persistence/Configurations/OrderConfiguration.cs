using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SS.GiftShop.Core;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Persistence.Configurations
{
    public class OrderConfiguration : IEntityTypeConfiguration<Order>
    {
        public void Configure(EntityTypeBuilder<Order> builder)
        {
            builder.HasMany(c => c.OrderDetails)
                .WithOne(c => c.Order)
                .HasForeignKey(c => c.OrderId);

            builder.Property(c => c.OrderDate)
                .IsRequired();
                //.HasMaxLength(AppConstants.StandardValueLength);

            builder.Property(c => c.UserId)
                .HasMaxLength(AppConstants.StandardValueLength);
        }
    }
}

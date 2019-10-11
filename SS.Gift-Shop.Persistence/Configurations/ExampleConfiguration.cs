using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SS.GiftShop.Core;
using SS.GiftShop.Domain.Entities;

namespace SS.GiftShop.Persistence.Configurations
{
    public sealed class ExampleConfiguration : IEntityTypeConfiguration<Example>
    {
        public void Configure(EntityTypeBuilder<Example> builder)
        {
            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(AppConstants.StandardValueLength);

            builder.Property(x => x.NormalizedName)
                .IsRequired()
                .HasMaxLength(AppConstants.StandardValueLength);

            builder.HasIndex(x => x.NormalizedName)
                .IsUnique();

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(AppConstants.EmailLength);
        }
    }
}

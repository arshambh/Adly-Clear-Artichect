using Adly.Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adly.Infrastructure.Persistence.Configurations.Category;

public class CategoryEntityConfiguration : IEntityTypeConfiguration<CategoryEntity>
{
    public void Configure(EntityTypeBuilder<CategoryEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100);

        builder.HasIndex(x => x.Name);

        builder.HasMany(x => x.Ads)
            .WithOne(x => x.Category)
            .HasForeignKey(x => x.CategoryId);

        builder.ToTable("Categories", "ad");

    }
}
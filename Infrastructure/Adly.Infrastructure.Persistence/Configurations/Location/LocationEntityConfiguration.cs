using Adly.Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Adly.Infrastructure.Persistence.Configurations.Location;

internal class LocationEntityConfiguration : IEntityTypeConfiguration<LocationEntity>
{
    public void Configure(EntityTypeBuilder<LocationEntity> builder)
    {
        builder.HasKey(x => x.Id);

        builder.Property(x => x.Name).HasMaxLength(100);

        builder.HasIndex(x => x.Name);

        builder.HasMany(x => x.Ads)
            .WithOne(x => x.Location)
            .HasForeignKey(x => x.LocationId);

        builder.ToTable("Locations", "ad");

    }
}
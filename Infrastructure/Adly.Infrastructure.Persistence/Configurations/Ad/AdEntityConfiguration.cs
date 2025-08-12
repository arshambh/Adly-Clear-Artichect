using Adly.Domain.Entities.Ad;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace Adly.Infrastructure.Persistence.Configurations.Ad;

internal class AdEntityConfiguration:IEntityTypeConfiguration<AdEntity>
{
    public void Configure(EntityTypeBuilder<AdEntity> builder)
    {
        builder.HasKey(c => c.Id);

        builder.Property(x => x.Title).HasMaxLength(100);

        builder.Property(x => x.Description).HasMaxLength(2048);

        builder.HasOne(x => x.Category)
            .WithMany(x => x.Ads)
            .HasForeignKey(x => x.CategoryId);

        builder.HasOne(x => x.Location)
            .WithMany(x => x.Ads)
            .HasForeignKey(x => x.LocationId);


        builder.HasOne(x => x.User)
            .WithMany(x => x.Ads)
            .HasForeignKey(x => x.UserId);

        builder.Property(x => x.CurrentState)
            .HasConversion<EnumToStringConverter<AdEntity.AdState>>()
            .HasMaxLength(20);

        builder.HasIndex(x => x.Title);
        builder.HasIndex(x => x.CurrentState);

        builder.OwnsMany(x => x.Images, navigationBuilder =>
        {
            navigationBuilder.ToJson("Images");
        });

        builder.OwnsMany(x => x.ChangeLogs, navigationBuilder =>
        {
            navigationBuilder.ToJson("ChangeLogs");
        });

        builder.ToTable("Ads", "ad");

    }
}
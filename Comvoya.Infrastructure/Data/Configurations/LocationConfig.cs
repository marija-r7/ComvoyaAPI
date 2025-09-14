using Comvoya.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Comvoya.Infrastructure.Data.Configurations
{
    public class LocationConfig : IEntityTypeConfiguration<Location>
    {
        public void Configure(EntityTypeBuilder<Location> b)
        {
            b.ToTable("Location");

            b.HasKey(x => x.Id);
            b.Property(x => x.Name).HasMaxLength(150).IsRequired();
            b.Property(x => x.Admin1).HasMaxLength(150);
            b.Property(x => x.Admin2).HasMaxLength(150);
            b.Property(x => x.CountryIso2).HasColumnType("char(2)").IsRequired();
            b.Property(x => x.Timezone).HasMaxLength(64);
            b.Property(x => x.Provider).HasMaxLength(30);
            b.Property(x => x.ProviderPlaceId).HasMaxLength(120);
            b.Property(x => x.CreatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");
            b.Property(x => x.UpdatedAtUtc).HasDefaultValueSql("SYSUTCDATETIME()");

            b.HasOne(x => x.Country)
             .WithMany(c => c.Locations)
             .HasForeignKey(x => x.CountryIso2)
             .OnDelete(DeleteBehavior.Restrict);

            b.HasIndex(x => new { x.CountryIso2, x.Name });
            b.HasIndex(x => x.Name);
            b.HasIndex(x => new { x.Admin1, x.Name });
            b.HasIndex(x => new { x.Latitude, x.Longitude });

            b.HasIndex(x => new { x.Provider, x.ProviderPlaceId })
             .IsUnique()
             .HasFilter("[Provider] IS NOT NULL AND [ProviderPlaceId] IS NOT NULL");
        }
    }
}
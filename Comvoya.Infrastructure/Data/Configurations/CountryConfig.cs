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
    public class CountryConfig : IEntityTypeConfiguration<Country>
    {
        public void Configure(EntityTypeBuilder<Country> b)
        {
            b.ToTable("Country");

            b.HasKey(x => x.Iso2);
            b.Property(x => x.Iso2).HasColumnType("char(2)").IsRequired();
            b.Property(x => x.Iso3).HasColumnType("char(3)").IsRequired();
            b.Property(x => x.Name).HasMaxLength(100).IsRequired();
            b.Property(x => x.Emoji).HasMaxLength(4);

            b.HasIndex(x => x.Iso3).IsUnique();
            b.HasIndex(x => x.Name).IsUnique();
        }
    }
}
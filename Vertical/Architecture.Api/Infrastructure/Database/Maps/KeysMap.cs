using Architecture.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Architecture.Api.Infrastructure.Database.Maps
{
    public class KeysMap : IEntityTypeConfiguration<Keys>
    {
        public void Configure(EntityTypeBuilder<Keys> builder)
        {
            builder.HasKey(x => x.Table);
            builder.Property(x => x.Table)
                .HasMaxLength(50)
                .IsRequired();
            builder.Property(x => x.Value).IsRequired();
        }
    }
}
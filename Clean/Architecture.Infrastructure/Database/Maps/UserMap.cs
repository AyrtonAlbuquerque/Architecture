using Architecture.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Architecture.Infrastructure.Database.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Email)
                .HasMaxLength(100)
                .IsRequired();
            builder.Property(x => x.Password)
                .HasMaxLength(100)
                .IsRequired();

            builder.HasIndex(x => x.Email).IsUnique();
        }
    }
}
using Architecture.Api.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Architecture.Api.Domain.Maps
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            // Primary Key
            builder.HasKey(x => x.Id);

            // Properties
            builder.HasIndex(x => x.Email)
                .IsUnique();
            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(100);
        }
    }
}
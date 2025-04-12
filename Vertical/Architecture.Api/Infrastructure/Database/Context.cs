using Architecture.Api.Domain.Entities;
using Architecture.Api.Domain.Maps;
using Microsoft.EntityFrameworkCore;

namespace Architecture.Api.Infrastructure.Database
{
    public class Context(DbContextOptions options) : DbContext(options)
    {
        public DbSet<User> User => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(typeof(UserMap).Assembly);
        }
    }
}
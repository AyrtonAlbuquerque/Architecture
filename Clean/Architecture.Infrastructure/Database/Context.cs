using Architecture.Domain.Maps;
using Architecture.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Architecture.Infrastructure.Database
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
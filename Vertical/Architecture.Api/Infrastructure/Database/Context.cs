using Architecture.Api.Domain.Models;
using Architecture.Api.Infrastructure.Database.Maps;
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
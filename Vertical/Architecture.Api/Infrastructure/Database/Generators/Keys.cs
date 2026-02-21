using System.Data;
using System.Numerics;
using Architecture.Api.Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Architecture.Api.Infrastructure.Database.Generators
{
    public class Keys<TKey> : ValueGenerator<TKey> where TKey : struct, INumber<TKey>
    {
        public override bool GeneratesTemporaryValues => false;

        public override TKey Next(EntityEntry entry)
        {
            using (var context = (DbContext)Activator.CreateInstance(entry.Context.GetType(), new object[] { entry.Context.GetService<DbContextOptions>() })!)
            using (var transaction = context.Database.BeginTransaction(IsolationLevel.Serializable))
            {
                var table = entry.Metadata.GetTableName()!;
                var key = context.Set<Keys>().FirstOrDefault(x => x.Table == table) ??
                            throw new InvalidOperationException($"No entry found in Keys table for Table = '{table}'.");
                var value = key.Value;

                key.Value++;
                context.SaveChanges();
                transaction.Commit();

                return TKey.CreateChecked(value);
            }
        }
    }
}
using System.Linq.Expressions;
using System.Numerics;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace Architecture.Infrastructure.Database.Generators
{
    public class Increment<TKey> : ValueGenerator<TKey> where TKey : struct, INumber<TKey>
    {
        public override bool GeneratesTemporaryValues => false;

        public override TKey Next(EntityEntry entry)
        {
            var property = entry.Property(entry.CurrentValues.Properties
                .First(p => p.GetValueGeneratorFactory() != null || p.ValueGenerated != Microsoft.EntityFrameworkCore.Metadata.ValueGenerated.Never).Name);
            var parameter = Expression.Parameter(entry.Metadata.ClrType, "x");
            var max = (TKey?)typeof(Queryable).GetMethods()
                .First(m => m.Name == nameof(Queryable.Max) && m.GetParameters().Length == 2 && m.GetGenericArguments().Length == 2)
                .MakeGenericMethod(entry.Metadata.ClrType, typeof(TKey?))
                .Invoke(null, [(IQueryable)typeof(EntityFrameworkQueryableExtensions)
                        .GetMethod(nameof(EntityFrameworkQueryableExtensions.AsNoTracking))!
                        .MakeGenericMethod(entry.Metadata.ClrType)
                        .Invoke(null, [(IQueryable)typeof(DbContext)
                            .GetMethod(nameof(DbContext.Set), Type.EmptyTypes)!
                            .MakeGenericMethod(entry.Metadata.ClrType)
                            .Invoke(entry.Context, null)!])!,
                    Expression.Lambda(Expression.Convert(Expression.Property(parameter, property.Metadata.Name), typeof(TKey?)), parameter)]);
            var tracked = entry.Context.ChangeTracker
                .Entries()
                .Where(e => e.Metadata.ClrType == entry.Metadata.ClrType && e.State == EntityState.Added)
                .Select(e => e.Property(property.Metadata.Name).CurrentValue as TKey?)
                .Where(v => v.HasValue)
                .Select(v => v!.Value)
                .DefaultIfEmpty(TKey.Zero)
                .Max();

            return TKey.Max(max ?? TKey.Zero, tracked) + TKey.One;
        }
    }
}
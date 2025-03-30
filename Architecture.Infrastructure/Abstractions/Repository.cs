using Architecture.Domain.Abstractions;
using Architecture.Infrastructure.Database;
using Microsoft.EntityFrameworkCore;

namespace Architecture.Infrastructure.Abstractions
{
        public abstract class Repository<T>(Context context) : IRepository<T> where T : class
    {
        private readonly DbSet<T> entity = context.Set<T>();

        public int Count()
        {
            return entity.Count();
        }

        protected int Count(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            return filter(entity.AsQueryable<T>()).Count();
        }

        public async Task<int> CountAsync()
        {
            return await entity.CountAsync();
        }

        protected async Task<int> CountAsync(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            return await filter(entity.AsQueryable<T>()).CountAsync();
        }

        protected bool Exists(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            return filter(entity.AsQueryable<T>()).Any();
        }

        public async Task<bool> ExistsAsync(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            return await filter(entity.AsQueryable<T>()).AnyAsync();
        }

        public T Get(object id)
        {
            return entity.Find(id);
        }

        protected T Get(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            return filter(entity.AsQueryable<T>()).SingleOrDefault();
        }

        public async Task<T> GetAsync(object id)
        {
            return await entity.FindAsync(id);
        }

        protected async Task<T> GetAsync(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            return await filter(entity.AsQueryable<T>()).SingleOrDefaultAsync();
        }

        public IList<T> Select()
        {
            return entity.AsQueryable<T>().ToList();
        }

        protected IList<T> Select(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            return filter(entity.AsQueryable<T>()).ToList();
        }

        public async Task<IList<T>> SelectAsync()
        {
            return await entity.AsQueryable<T>().ToListAsync();
        }

        protected async Task<IList<T>> SelectAsync(Func<IQueryable<T>, IQueryable<T>> filter)
        {
            ArgumentNullException.ThrowIfNull(filter);
            return await filter(entity.AsQueryable<T>()).ToListAsync();
        }

        public int Execute(FormattableString query)
        {
            ArgumentNullException.ThrowIfNull(query);
            return context.Database.ExecuteSql(query);
        }

        public async Task<int> ExecuteAsync(FormattableString query)
        {
            ArgumentNullException.ThrowIfNull(query);
            return await context.Database.ExecuteSqlAsync(query);
        }

        public IQueryable<T> Query(FormattableString query)
        {
            ArgumentNullException.ThrowIfNull(query);
            return context.Database.SqlQuery<T>(query);
        }

        public T Insert(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            entity.Attach(item);

            return item;
        }

        public IList<T> Insert(IList<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);

            if (!items.Any()) return Enumerable.Empty<T>().ToList();

            entity.AttachRange(items);

            return items;
        }

        public T Update(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            entity.Update(item);

            return item;
        }

        public IList<T> Update(IList<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);

            if (items.Count == 0) return Enumerable.Empty<T>().ToList();

            entity.UpdateRange(items);

            return items;
        }

        public T Delete(T item)
        {
            ArgumentNullException.ThrowIfNull(item);

            entity.Remove(item);

            return item;
        }

        public IList<T> Delete(IList<T> items)
        {
            ArgumentNullException.ThrowIfNull(items);

            if (items.Count == 0) return Enumerable.Empty<T>().ToList();

            entity.RemoveRange(items);

            return items;
        }

        public IStoredProcedure<T> StoredProcedure(string procedureName)
        {
            return new StoredProcedure<T>(context, procedureName);
        }

        public IStoredProcedure<TResult> StoredProcedure<TResult>(string procedureName) where TResult : class
        {
            return new StoredProcedure<TResult>(context, procedureName);
        }

        public ICommand Command(string commandText)
        {
            return new Command(context, commandText);
        }

        public void Save()
        {
            context.SaveChanges();
        }

        public async Task SaveAsync()
        {
            await context.SaveChangesAsync();
        }
    }
}
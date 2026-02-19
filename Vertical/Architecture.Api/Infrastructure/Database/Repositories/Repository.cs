using System.Data;
using System.Data.Common;
using Architecture.Api.Domain.Interfaces;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Architecture.Api.Infrastructure.Database.Repositories
{
    public class Command : ICommand
    {
        private readonly DbCommand command;

        public Command(Context context, string commandText)
        {
            command = context.Database.GetDbConnection().CreateCommand();
            command.CommandText = commandText;
            command.CommandType = CommandType.Text;
        }

        public ICommand SetParameter(string name, object? value)
        {
            var parameter = command.CreateParameter();

            parameter.ParameterName = name.StartsWith("@") ? name : "@" + name;
            parameter.Value = value;

            if (command.CommandType == CommandType.Text && command.CommandText.Contains(parameter.ParameterName))
                command.CommandText = command.CommandText.Replace
                (
                    name.Replace("]", "]]")
                        .Replace("'", "''")
                        .Replace("--", string.Empty)
                        .Replace("/*", string.Empty)
                        .Replace("*/", string.Empty)
                        .Replace(";", string.Empty),
                    value.ToString()
                        .Replace("]", "]]")
                        .Replace("'", "''")
                        .Replace("--", string.Empty)
                        .Replace("/*", string.Empty)
                        .Replace("*/", string.Empty)
                        .Replace(";", string.Empty)
                );

            command.Parameters.Add(parameter);

            return this;
        }

        public ICommand Text(string text)
        {
            command.CommandText = text;
            return this;
        }

        public ICommand Timeout(int timeout)
        {
            command.CommandTimeout = timeout;
            return this;
        }

        public ICommand Type(CommandType type)
        {
            command.CommandType = type;
            return this;
        }

        public int ExecuteNonQuery()
        {
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            var result = command.ExecuteNonQuery();

            command.Dispose();

            return result;
        }

        public async Task<int> ExecuteNonQueryAsync()
        {
            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteNonQueryAsync();

            await command.DisposeAsync();

            return result;
        }

        public DbDataReader ExecuteReader()
        {
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            var result = command.ExecuteReader();

            command.Dispose();

            return result;
        }

        public async Task<DbDataReader> ExecuteReaderAsync()
        {
            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteReaderAsync();

            await command.DisposeAsync();

            return result;
        }

        public object? ExecuteScalar()
        {
            if (command.Connection.State != ConnectionState.Open)
                command.Connection.Open();

            var result = command.ExecuteScalar();

            command.Dispose();

            return result;
        }

        public async Task<object?> ExecuteScalarAsync()
        {
            if (command.Connection.State != ConnectionState.Open)
                await command.Connection.OpenAsync();

            var result = await command.ExecuteScalarAsync();

            await command.DisposeAsync();

            return result;
        }
    }

    public class StoredProcedure<T>(Context context, string name) : IStoredProcedure<T> where T : class
    {
        private readonly List<SqlParameter> parameters = new List<SqlParameter>();

        public IStoredProcedure<T> SetParameter(string name, object? value, SqlDbType? type = null, int? size = null, byte? precision = null, byte? scale = null, ParameterDirection direction = ParameterDirection.Input)
        {
            var parameter = new SqlParameter();

            parameter.ParameterName = name.StartsWith("@") ? name : "@" + name;
            parameter.Value = value;

            if (type.HasValue)
                parameter.SqlDbType = type.Value;

            if (size.HasValue)
                parameter.Size = size.Value;

            if (precision.HasValue)
                parameter.Precision = precision.Value;

            if (scale.HasValue)
                parameter.Scale = scale.Value;

            parameters.Add(parameter);

            return this;
        }

        public int Execute()
        {
            return context.Database.ExecuteSqlRaw(Sql(), parameters.ToArray());
        }

        public async Task<int> ExecuteAsync()
        {
            return await context.Database.ExecuteSqlRawAsync(Sql(), parameters.ToArray());
        }

        public T Unique()
        {
            return context.Database.SqlQueryRaw<T>(Sql(), parameters.ToArray())
                .ToList()
                .FirstOrDefault();
        }

        public async Task<T> UniqueAsync()
        {
            return (await context.Database.SqlQueryRaw<T>(Sql(), parameters.ToArray())
                .ToListAsync())
                .FirstOrDefault();
        }

        public IList<T> Result()
        {
            return context.Database.SqlQueryRaw<T>(Sql(), parameters.ToArray()).ToList();
        }

        public async Task<IList<T>> ResultAsync()
        {
            return await context.Database.SqlQueryRaw<T>(Sql(), parameters.ToArray()).ToListAsync();
        }

        private string Sql()
        {
            var list = parameters.Count > 0
                ? string.Join(", ", parameters.Select(p => p.ParameterName))
                : string.Empty;

            return $"EXEC {name} {list}";
        }
    }

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
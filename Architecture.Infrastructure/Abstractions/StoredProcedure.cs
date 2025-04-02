using System.Data;
using Architecture.Domain.Abstractions;
using Architecture.Infrastructure.Database;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace Architecture.Infrastructure.Abstractions
{
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
}
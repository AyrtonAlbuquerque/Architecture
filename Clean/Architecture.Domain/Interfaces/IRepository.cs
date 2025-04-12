using System.Data;
using System.Data.Common;

namespace Architecture.Domain.Interfaces
{
    public interface ICommand
    {
        ICommand SetParameter(string name, object? value);
        ICommand Type(CommandType type);
        ICommand Text(string text);
        ICommand Timeout(int timeout);
        int ExecuteNonQuery();
        DbDataReader ExecuteReader();
        object? ExecuteScalar();
        Task<int> ExecuteNonQueryAsync();
        Task<DbDataReader> ExecuteReaderAsync();
        Task<object?> ExecuteScalarAsync();
    }

    public interface IStoredProcedure<T> where T : class
    {
        IStoredProcedure<T> SetParameter(string name, object? value, SqlDbType? type = null, int? size = null, byte? precision = null, byte? scale = null, ParameterDirection direction = ParameterDirection.Input);
        int Execute();
        T Unique();
        IList<T> Result();
        Task<int> ExecuteAsync();
        Task<T> UniqueAsync();
        Task<IList<T>> ResultAsync();
    }

    public interface IRepository<T> where T : class
    {
        int Count();
        Task<int> CountAsync();
        T Get(object id);
        Task<T> GetAsync(object id);
        IList<T> Select();
        Task<IList<T>> SelectAsync();
        int Execute(FormattableString query);
        Task<int> ExecuteAsync(FormattableString query);
        IQueryable<T> Query(FormattableString query);
        T Insert(T item);
        IList<T> Insert(IList<T> items);
        T Update(T item);
        IList<T> Update(IList<T> items);
        T Delete(T item);
        IList<T> Delete(IList<T> items);
        IStoredProcedure<T> StoredProcedure(string procedureName);
        IStoredProcedure<TResult> StoredProcedure<TResult>(string procedureName) where TResult : class;
        ICommand Command(string commandText);
        void Save();
        Task SaveAsync();
    }
}
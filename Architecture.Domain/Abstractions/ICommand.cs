using System.Data;
using System.Data.Common;

namespace Architecture.Domain.Abstractions
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

}
using System.Data;

namespace Architecture.Domain.Abstractions
{
    public interface IStoredProcedure<T> where T : class
    {
        IStoredProcedure<T> Parameter(string name, object? value, SqlDbType? type = null, int? size = null, byte? precision = null, byte? scale = null, ParameterDirection direction = ParameterDirection.Input);
        int Execute();
        T Unique();
        IList<T> Result();
        Task<int> ExecuteAsync();
        Task<T> UniqueAsync();
        Task<IList<T>> ResultAsync();
    }

}
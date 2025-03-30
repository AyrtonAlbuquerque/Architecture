namespace Architecture.Domain.Abstractions
{
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
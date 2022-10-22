namespace r7.Repositories
{
    public interface IQuery
    {
        Task<int> ExecuteAsync(string sql);
        Task<int> ExecuteAsync(string sql, object parameters);
        Task<T> ExecuteScalarAsync<T>(string sql, object parameters);
        Task<IEnumerable<T>> QueryAsync<T>(string sql);
        Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters);
        Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters);
    }
}

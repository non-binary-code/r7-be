using Dapper;

namespace r7.Repositories
{
    public class DapperQuery : IQuery
    {
        private readonly IDbConnectionFactory _dbConnectionFactory;

        public DapperQuery(IDbConnectionFactory dbConnectionFactory) =>
            _dbConnectionFactory = dbConnectionFactory;

        public async Task<int> ExecuteAsync(string sql)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql);
        }

        public async Task<int> ExecuteAsync(string sql, object parameters)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteAsync(sql, parameters);
        }

        public async Task<T> ExecuteScalarAsync<T>(string sql, object parameters)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.ExecuteScalarAsync<T>(sql, parameters);
        }

        public async Task<T> QueryFirstOrDefaultAsync<T>(string sql, object parameters)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<T>(sql, parameters);
        }

        public async Task<IEnumerable<T>> QueryAsync<T>(string sql)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync<T>(sql);
        }
        
        public async Task<IEnumerable<T>> QueryAsync<T>(string sql, object parameters)
        {
            using var connection = _dbConnectionFactory.CreateConnection();
            return await connection.QueryAsync<T>(sql, parameters);
        }
    }
}

using System.Data;
using Npgsql;
using Serilog;

namespace r7.Repositories
{
    public class DbConnectionFactory : IDbConnectionFactory
    {
        public IDbConnection CreateConnection()
        {
            var connection = new NpgsqlConnection(CreateDbConnectionString());
            connection.Open();
            return connection;
        }

        private static string CreateDbConnectionString()
        {
            var databaseUrl = Environment.GetEnvironmentVariable("DATABASE_URL");
            Log.Information($"Database: {databaseUrl}");

            var main = databaseUrl.Split("/")[2];
            var user = main.Split(":")[0];
            var pass = main.Split(":")[1].Split("@")[0];
            var host = main.Split("@")[1].Split(":")[0];
            var db = databaseUrl.Split("/")[3];

            Log.Information($"user: {user} pass: {pass} host: {host} db: {db}");

            var builder = new NpgsqlConnectionStringBuilder
            {
                Host = host,
                Port = 5432,
                Username = user,
                Password = pass,
                Database = db,
                Pooling = false
            };

            return builder.ToString();
        }
    }
}

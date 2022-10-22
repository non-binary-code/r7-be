using System.Data;

namespace r7.Repositories
{
    public interface IDbConnectionFactory
    {
        public IDbConnection CreateConnection();
    }
}

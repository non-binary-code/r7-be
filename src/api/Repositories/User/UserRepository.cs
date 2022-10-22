using r7.Models;
using Serilog;

namespace r7.Repositories
{
    public class UserRepository : IUserRepository
    {
        private IQuery _query;

        public UserRepository(IQuery query)
        {
            _query = query;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            var sqlStatement = GetAllUsersSqlStatement();
            var users = await _query.QueryAsync<User>(sqlStatement);

            return users;
        }

        public async Task<User> GetUserByUserId(long userId)
        {
            var sql = GetUserSqlStatement();
            var user = await _query.QueryFirstOrDefaultAsync<User>(sql, new
            {
                UserId = userId
            });

            return user;
        }

        public async Task<long> AddUser(User user)
        {
            var sql = AddUserSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, user);

            return id;
        }

        public async Task EditUser(User user)
        {
            var sql = EditUserSqlStatement();
            await _query.ExecuteAsync(sql, user);
        }

        private static string GetAllUsersSqlStatement()
        {
            return $@"SELECT Id, Username, Email, Bio, PictureUrl FROM Users";
        }

        private static string GetUserSqlStatement()
        {
            return $@"
            SELECT id, username, email, pictureurl, bio
            FROM users 
            WHERE id = @UserId";
        }

        private static string AddUserSqlStatement()
        {
            return $@"INSERT INTO Users
                 (
                   Username, Email, Bio, PictureUrl
                 )
                 VALUES
                 (
                   @Username, @Email, @Bio, @PictureUrl
                 ) RETURNING Id";
        }

        private static string EditUserSqlStatement()
        {
            return $@"UPDATE Users
                    SET
                    Username = @Username,
                    Email = @Email,
                    Bio = @Bio,
                    PictureUrl = @PictureUrl
                    WHERE Id = @Id";
        }
    }
}
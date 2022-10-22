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

        public async Task<IEnumerable<Item>> GetItemsByUserId(long userId)
        {
            var sql = GetItemsByUserIdSqlStatement();
            var items = await _query.QueryAsync<Item>(sql, new
            {
                UserId = userId
            });

            return items;
        }

        public async Task<User> AddUser(NewUserRequest user)
        {
            var sql = AddUserSqlStatement();
            var id = await _query.ExecuteScalarAsync<long>(sql, user);

            return await GetUserByUserId(id);
        }

        public async Task EditUser(User user)
        {
            var sql = EditUserSqlStatement();
            await _query.ExecuteAsync(sql, user);
        }

        private static string GetAllUsersSqlStatement()
        {
            return $@"SELECT Id, Username, Email, Bio, PictureUrl FROM users";
        }

        private static string GetUserSqlStatement()
        {
            return $@"
            SELECT Id, Username, Email, Bio, PictureUrl FROM users
            WHERE id = @UserId";
        }

        private static string GetItemsByUserIdSqlStatement()
        {
            return $@"
            SELECT i.Id, i.Name, i.Description, i.CategoryTypeId, i.ConditionTypeId, i.Delivery, i.Collection, i.Postage, i.Recover, i.PictureUrl, i.Location FROM items i
            LEFT JOIN useritems ui ON i.id = ui.itemid
            LEFT JOIN users u ON u.id = ui.userid
            WHERE u.id = @UserId";
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
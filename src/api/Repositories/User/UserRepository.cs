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

        public async Task<IEnumerable<User>> GetUsers(UserQueryParameters queryParameters)
        {
            var sqlStatement = GetAllUsersSqlStatement();
            var willRecover = queryParameters.Recover ? "TRUE" : "FALSE";

            sqlStatement += $" WHERE WillRecover = {willRecover} ORDER BY ID DESC";

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
            return $@"SELECT * FROM users";
        }

        private static string GetUserSqlStatement()
        {
            return $@"SELECT * FROM users WHERE id = @UserId";
        }

        private static string GetItemsByUserIdSqlStatement()
        {
            return $@"SELECT * FROM items WHERE CurrentUserId = @UserId";
        }

        private static string AddUserSqlStatement()
        {
            return $@"INSERT INTO Users
                 (
                   Username, Email, Bio, PictureUrl, Location, WillRecover, AllowBookings, Availability, DistanceWillTravel
                 )
                 VALUES
                 (
                   @Username, @Email, @Bio, @PictureUrl, @Location, @WillRecover, @AllowBookings, @Availability, @DistanceWillTravel
                 ) RETURNING Id";
        }

        private static string EditUserSqlStatement()
        {
            return $@"UPDATE Users
                    SET
                    Username = @Username,
                    Email = @Email,
                    Bio = @Bio,
                    PictureUrl = @PictureUrl,
                    Location = @Location,
                    WillRecover = @WillRecover,
                    AllowBookings = @AllowBookings,
                    Availability = @Availability,
                    DistanceWillTravel = @DistanceWillTravel
                    WHERE Id = @Id";
        }
    }
}
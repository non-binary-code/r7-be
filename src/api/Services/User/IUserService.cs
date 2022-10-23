using r7.Models;

namespace r7.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers(UserQueryParameters queryParameters);
    Task<User> GetUserByUserId(long userId);
    Task<IEnumerable<Item>> GetItemsByUserId(long userId);
    Task<User> AddUser(NewUserRequest user);
    Task EditUser(User user);
}
using r7.Models;

namespace r7.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUserByUserId(long userId);
    Task<User> AddUser(NewUserRequest user);
    Task EditUser(User user);
}
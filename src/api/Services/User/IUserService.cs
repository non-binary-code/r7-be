using r7.Models;

namespace r7.Services;

public interface IUserService
{
    Task<IEnumerable<User>> GetUsers();
    Task<User> GetUserByUserId(long userId);
    Task<long> AddUser(User user);
    Task EditUser(User user);
}
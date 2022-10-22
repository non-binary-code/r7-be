using r7.Models;

namespace r7.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserByUserId(long userId);
        Task<User> AddUser(NewUserRequest user);
        Task EditUser(User user);
    }
}
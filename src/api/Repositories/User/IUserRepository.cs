using r7.Models;

namespace r7.Repositories
{
    public interface IUserRepository
    {
        Task<IEnumerable<User>> GetUsers();
        Task<User> GetUserByUserId(long userId);
        Task<long> AddUser(User user);
        Task EditUser(User user);
    }
}
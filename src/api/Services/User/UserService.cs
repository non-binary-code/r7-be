using r7.Models;
using r7.Repositories;

namespace r7.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;

        public UserService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<IEnumerable<User>> GetUsers()
        {
            return await _userRepository.GetUsers();
        }

        public async Task<User> GetUserByUserId(long userId)
        {
            return await _userRepository.GetUserByUserId(userId);
        }

        public async Task<long> AddUser(User user)
        {
            return 1;
            //return await _userRepository.AddUser(user);
        }

        public async Task EditUser(User user)
        {
            //await _userRepository.EditUser(user);
        }
    }
}
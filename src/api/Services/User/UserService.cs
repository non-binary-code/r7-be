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
            return new List<User>()
            {
                new User()
                {
                    Id = 1,
                    Email = "test1@test.com",
                    Username = "Mr Test Tester1",
                    PictureUrl = "www.test.com/picture/1",
                    Bio = "Test Bio"
                },
                new User()
                {
                    Id = 2,
                    Email = "test2@test.com",
                    Username = "Mr Test Tester2",
                    PictureUrl = "www.test.com/picture/2",
                    Bio = "Test Bio 2"
                },
                new User()
                {
                    Id = 3,
                    Email = "test3@test.com",
                    Username = "Mr Test Tester3",
                    PictureUrl = "www.test.com/picture/3",
                    Bio = "Test Bio 3"
                }
            };
        }

        public async Task<User> GetUserByUserId(long userId)
        {
            return new User()
            {
                Id = 1,
                Email = "test@test.com",
                Username = "Mr Test Tester",
                PictureUrl = "www.test.com/picture",
                Bio = "Test Bio"
            };
            //return await _userRepository.GetUserByUserId(userId);
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
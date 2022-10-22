using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using r7.Models;
using r7.Services;

namespace r7
{
    [Route("api/[controller]")]
    [ApiController]
    [AllowAnonymous]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(IEnumerable<User>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> GetUsers()
        {
            try
            {
                var returnedUsers = await _userService.GetUsers();
                return Ok(returnedUsers);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
        
        [HttpGet]
        [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        [Route("{userId}")]
        public async Task<ActionResult> GetUserByUserId(long userId)
        {
            try
            {
                var returnedUser = await _userService.GetUserByUserId(userId);
                return Ok(returnedUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
        
        [HttpPost]
        [ProducesResponseType(typeof(User), StatusCodes.Status201Created)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> AddUser([FromBody] NewUserRequest user)
        {
            try
            {
                var newUser = await _userService.AddUser(user);

                return Created($"{newUser.Id}", newUser);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
        
        [HttpPut]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(string), StatusCodes.Status400BadRequest)]
        public async Task<ActionResult> EditUser([FromBody] User user)
        {
            try
            {
                await _userService.EditUser(user);
                return Ok();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return BadRequest(e);
            }
        }
    }
}
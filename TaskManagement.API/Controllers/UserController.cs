using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetUserById(string userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{userId}")]
        //[Authorize(Roles = StaticUserRoles.ADMIN)] 
        public async Task<IActionResult> DeleteUser(string userId)
        {
            var deleteResult = await _userService.DeleteUserByIdAsync(userId);

            if (deleteResult.IsSucceed)
            {
                return Ok(deleteResult);
            }

            return BadRequest(deleteResult);
        }
    }
}

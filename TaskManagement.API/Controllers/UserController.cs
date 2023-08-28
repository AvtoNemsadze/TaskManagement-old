using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.OtherObjects;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
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
        public async Task<IActionResult> GetUserById(int userId)
        {
            var user = await _userService.GetUserByIdAsync(userId);

            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        [HttpDelete("{userId}")]
        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        public async Task<IActionResult> DeleteUserById(int userId)
        {
            var response = await _userService.DeleteUserByIdAsync(userId);

            if (!response.IsSucceed)
            {
                return NotFound(response);
            }

            return Ok(response);
        }
    }
}

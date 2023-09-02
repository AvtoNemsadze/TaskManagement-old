using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.OtherObjects;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    //[Authorize]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        public UserController(IUserService userService)
        {
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        [HttpGet]
        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = await _userService.GetAllUsersAsync();
            return Ok(users);
        }

        [HttpGet("{userId}")]
        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
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
        [Authorize(Roles = SystemRoles.SUPERADMIN)]
        public async Task<IActionResult> DeleteUserById(int userId)
        {
            var response = await _userService.DeleteUserByIdAsync(userId);

            if (!response.IsSucceed)
            {
                return NotFound(response);
            }

            return Ok(response);
        }

        // update user
        [HttpPut("update/{userId}")]
        public async Task<IActionResult> UpdateUser(int userId, [FromBody] UpdateUserDto updateUserDto)
        {
            var response = await _userService.UpdateUserAsync(userId, updateUserDto);

            if (!response.IsSucceed)
            {
                return BadRequest(response);
            }

            return Ok(response);
        }

        // update user's password
        [HttpPut("changepassword/{userId}")]
        public async Task<IActionResult> ChangePassword(int userId, string currentPassword, string newPassword)
        {
            var result = await _userService.ChangePasswordAsync(userId, currentPassword, newPassword);

            if (result.IsSucceed)
            {
                return Ok(new { result.Message });
            }

            else
            {
                return BadRequest(new { result.Message });
            }
        }
    }
}

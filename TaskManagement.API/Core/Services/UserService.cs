using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Core.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UserService(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
        }

        public async Task<List<UserDto>> GetAllUsersAsync()
        {
            var users = await _userManager.Users
            .Select(u => new UserDto
            {
                FirstName = u.FirstName,
                LastName = u.LastName,
                UserName = u.UserName,
                Email = u.Email,
                Id = u.Id,
            })
            .ToListAsync();

            return users;
        }
// here s code
        public async Task<UserDto> GetUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            var userDto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id
            };

            return userDto;
        }

        public async Task<AuthServiceResponseDto> DeleteUserByIdAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);

            if (user == null)
            {
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "User not found." };
            }

            var result = await _userManager.DeleteAsync(user);

            if (result.Succeeded)
            {
                return new AuthServiceResponseDto() { IsSucceed = true, Message = "User deleted successfully." };
            }

            return new AuthServiceResponseDto() { IsSucceed = false, Message = "Error deleting user." };
        }
    }
}

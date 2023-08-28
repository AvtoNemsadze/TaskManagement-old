using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Core.DbContexts;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Core.Services
{
    public class UserService : IUserService
    {
        //private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;
        public UserService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        async Task<List<UserDto>> IUserService.GetAllUsersAsync()
        {
            var usersWithRoles = await _context.Users
                .Include(u => u.Role)
                .Select(u => new UserDto
                {
                    Id = u.Id,
                    FirstName = u.FirstName,
                    LastName = u.LastName,
                    UserName = u.UserName,
                    Email = u.Email,
                    Role = u.Role.RoleName  
                })
                .ToListAsync();

            return usersWithRoles;
        }


        public async Task<UserDto?> GetUserByIdAsync(int userId)
        {
            var user = await _context.Users
               .Include(u => u.Role)
               .FirstOrDefaultAsync(u => u.Id == userId);

            if (user == null)
            {
                return null;
            }

            var userDto = new UserDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                Email = user.Email,
                Id = user.Id,
                Role = user.Role.RoleName
            };

            return userDto;
        }

        public async Task<AuthServiceResponseDto> DeleteUserByIdAsync(int userId)
        {
            var userToDelete = await _context.Users.FindAsync(userId);

            if (userToDelete == null)
            {
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "User not found." };
            }

            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return new AuthServiceResponseDto() { IsSucceed = true, Message = "User deleted successfully." };
        }
    }
}

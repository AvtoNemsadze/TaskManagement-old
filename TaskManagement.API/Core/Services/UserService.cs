using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Core.DbContexts;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Interface;
using System.Security.Cryptography;
using TaskManagement.API.Core.Common;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Services
{
    public class UserService : IUserService
    {
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

        public async Task<UserServiceResponse> DeleteUserByIdAsync(int userId)
        {
            var userToDelete = await _context.Users.FindAsync(userId);

            if (userToDelete == null)
            {
                return new UserServiceResponse() { IsSucceed = false, Message = "User not found." };
            }

            // Find all comments associated with the user and delete it
            var userComments = _context.Comments.Where(c => c.UserId == userId);
            _context.Comments.RemoveRange(userComments);


            _context.Users.Remove(userToDelete);
            await _context.SaveChangesAsync();

            return new UserServiceResponse() { IsSucceed = true, Message = "User deleted successfully." };
        }

        // Get Users BY TeamId
        public async Task<List<ApplicationUser>> GetUsersByTeamId(int teamId)
        {
            return await _context.Users
                .Where(u => u.TeamId == teamId)
                .ToListAsync();
        }

        #region UpdateUserAsync
        public async Task<UserServiceResponse> UpdateUserAsync(int userId, UpdateUserDto updateUserDto)
        {
            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (userToUpdate == null)
            {
                return new UserServiceResponse() { IsSucceed = false, Message = "User not found." };
            }

            if (updateUserDto.FirstName != null)
            {
                userToUpdate.FirstName = updateUserDto.FirstName;
            }

            if (updateUserDto.LastName != null)
            {
                userToUpdate.LastName = updateUserDto.LastName;
            }

            if (updateUserDto.UserName != null)
            {
                userToUpdate.UserName = updateUserDto.UserName;
            }


            if (updateUserDto.Email != null)
            {
                userToUpdate.Email = updateUserDto.Email;
            }

            if (updateUserDto.RoleId > 0)
            {
                userToUpdate.RoleId = updateUserDto.RoleId;
            }

            await _context.SaveChangesAsync();

            return new UserServiceResponse() { IsSucceed = true, Message = "User updated successfully." };
        }
        #endregion

        #region ChangePasswordAsync
        public async Task<UserServiceResponse> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            if (currentPassword is null)
            {
                throw new ArgumentNullException(nameof(currentPassword));
            }

            if (newPassword is null)
            {
                throw new ArgumentNullException(nameof(newPassword));
            }


            var userToUpdate = await _context.Users.FirstOrDefaultAsync(u => u.Id == userId);

            if (userToUpdate == null)
            {
                return new UserServiceResponse() { IsSucceed = false, Message = "User not found." };
            }

            if (!VerifyPassword(userToUpdate.PasswordHash, currentPassword))
            {
                return new UserServiceResponse() { IsSucceed = false, Message = "Current password is incorrect." };
            }

            // Hash and update the new password
            var salt = GenerateSalt();
            userToUpdate.PasswordHash = HashPassword(newPassword, salt);
            userToUpdate.PasswordSalt = Convert.ToBase64String(salt);

            await _context.SaveChangesAsync();

            return new UserServiceResponse() { IsSucceed = true, Message = "Password changed successfully." };
        }

        private static bool VerifyPassword(string hashedPassword, string currentPassword)
        {
            byte[] hashBytes = Convert.FromBase64String(hashedPassword);

            byte[] salt = new byte[16];
            Array.Copy(hashBytes, 0, salt, 0, 16);

            using (var pbkdf2 = new Rfc2898DeriveBytes(currentPassword, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); 

                for (int i = 0; i < 32; i++)
                {
                    if (hashBytes[i + 16] != hash[i])
                    {
                        return false; 
                    }
                }

                return true;
            }
        }

        private static string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); 

                byte[] hashBytes = new byte[48]; 
                Array.Copy(salt, 0, hashBytes, 0, 16);
                Array.Copy(hash, 0, hashBytes, 16, 32);

                return Convert.ToBase64String(hashBytes);
            }
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
        #endregion
    }
}

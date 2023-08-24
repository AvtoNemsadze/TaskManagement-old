using Microsoft.AspNetCore.Identity;
using System.Security.Cryptography;
using TaskManagement.API.Core.DbContexts;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Enums;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Core.Services
{
    public class NewAuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        public NewAuthService(ApplicationDbContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }


        public Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthServiceResponseDto> MakeSuperAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            throw new NotImplementedException();
        }


        public Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            throw new NotImplementedException();
        }





        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistUser = await _context.Users.FindAsync(registerDto.UserName);

            if (isExistUser != null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "UserName Already Exsist" };

            var parsedUserRole = Enum.Parse<UserRoles>(registerDto.UserRole);
            var salt = GenerateSalt();
            var hashedPassword = HashPassword(registerDto.Password, salt);

            UserEntity newUser = new UserEntity()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PasswordHash = hashedPassword,
                PasswordSalt = Convert.ToBase64String(salt),
                RoleName = parsedUserRole.ToString(),
                CreatedDate = DateTime.UtcNow
            };

            _context.Users.Add(newUser);
            if (await _context.SaveChangesAsync() > 0)
            {
                return new AuthServiceResponseDto() { IsSucceed = true, Message = "User Created Successfully" };
            }
            else
            {
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "Failed to save user data." };
            }

        }

        private string HashPassword(string password, byte[] salt)
        {
            using (var pbkdf2 = new Rfc2898DeriveBytes(password, salt, 10000, HashAlgorithmName.SHA256))
            {
                byte[] hash = pbkdf2.GetBytes(32); // 32 bytes for a 256-bit hash

                // Combine the salt and hash into a single array
                byte[] hashBytes = new byte[48]; // 16 bytes salt + 32 bytes hash
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

    }
}

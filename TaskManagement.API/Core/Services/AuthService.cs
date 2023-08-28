using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TaskManagement.API.Core.DbContexts;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Enums;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.OtherObjects;

namespace TaskManagement.API.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        public AuthService(ApplicationDbContext context, IConfiguration configuration)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); ;
        }

        #region SeedRolesAsync
        public async Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            var existingRoles = await _context.Roles.ToListAsync();

            var rolesToSeed = RoleSeedData.Roles.Where(role => !existingRoles.Any(existingRole => existingRole.RoleName == role.RoleName));

            foreach (var role in rolesToSeed)
            {
                _context.Roles.AddRange(role);
            }

            await _context.SaveChangesAsync();

            return new AuthServiceResponseDto { IsSucceed = true, Message = "Roles seeded successfully." };
        }
        #endregion

        #region LoginAsync
        public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.UserName);
            if (user is null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "Invalid Credentials" };

            var hashedPassword = HashPassword(loginDto.Password, Convert.FromBase64String(user.PasswordSalt));

            if (hashedPassword != user.PasswordHash)
            {
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "Invalid Credentials" };
            }

            var userRoles = await _context.UserRoles
             .Where(ur => ur.UserId == user.Id)
             .Join(_context.Roles, ur => ur.RoleId, r => r.Id, (ur, r) => r.RoleName)
             .ToListAsync();

            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.UserName),
                new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new Claim("JWTID", Guid.NewGuid().ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName)
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var token = GenerateNewJsonWebToken(authClaims);

            return new AuthServiceResponseDto() { IsSucceed = true, Message = token };
        }

        private string GenerateNewJsonWebToken(List<Claim> claims)
        {
            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

            var tokenObject = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(1),
                    claims: claims,
                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
                );

            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

            return token;
        }
        #endregion

        #region RegisterAsync
        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            var isExistUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == registerDto.UserName);

            if (isExistUser != null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "UserName Already Exsist" };

            var salt = GenerateSalt();
            var hashedPassword = HashPassword(registerDto.Password, salt);

            ApplicationUser appUser = new ApplicationUser()
            {
                FirstName = registerDto.FirstName,
                LastName = registerDto.LastName,
                Email = registerDto.Email,
                UserName = registerDto.UserName,
                PasswordHash = hashedPassword,
                PasswordSalt = Convert.ToBase64String(salt),
                CreatedDate = DateTime.UtcNow,
                RoleId = registerDto.RoleId,
            };

            UserRoleEntity userRoles = new UserRoleEntity()
            {
                UserId = appUser.Id,
                RoleId = appUser.RoleId,
                UserName = appUser.UserName,
            };

            _context.Users.Add(appUser);
            _context.UserRoles.Add(userRoles);

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
        #endregion

        // user system roles

        #region MakeAdminAsync
        public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == updatePermissionDto.UserName);

            if (user is null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "Invalid User name !!!" };

            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == SystemRoles.ADMIN);
            if (existingRole == null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "Admin role not found." };

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            if (userRole == null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "User role not found." };

            if (userRole.RoleName != SystemRoles.ADMIN)
            {
                user.RoleId = existingRole.Id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // Update user's UserRoleEntity record
                var userRoles = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id);
                if (userRoles != null)
                {
                    userRoles.RoleId = existingRole.Id;
                    _context.UserRoles.Update(userRoles);
                    await _context.SaveChangesAsync();
                }

                return new AuthServiceResponseDto() { IsSucceed = true, Message = "Now user is an Admin" };
            }

            return new AuthServiceResponseDto() { IsSucceed = false, Message = "User is already an Admin" };
        }
        #endregion

        #region MakeSuperAdminAsync
        public async Task<AuthServiceResponseDto> MakeSuperAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == updatePermissionDto.UserName);

            if (user is null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "Invalid User name !!!" };

            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == SystemRoles.SUPERADMIN);
            if (existingRole == null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "SuperAdmin role not found." };

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            if (userRole == null)
                return new AuthServiceResponseDto() { IsSucceed = false, Message = "User role not found." };

            if (userRole.RoleName != SystemRoles.SUPERADMIN)
            {
                user.RoleId = existingRole.Id;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();

                // Update user's UserRoleEntity record
                var userRoles = await _context.UserRoles.FirstOrDefaultAsync(ur => ur.UserId == user.Id);
                if (userRoles != null)
                {
                    userRoles.RoleId = existingRole.Id;
                    _context.UserRoles.Update(userRoles);
                    await _context.SaveChangesAsync();
                }

                return new AuthServiceResponseDto() { IsSucceed = true, Message = "Now user is a SuperAdmin" };
            }

            return new AuthServiceResponseDto() { IsSucceed = false, Message = "User is already a SuperAdmin" };
        }
        #endregion
    }
}

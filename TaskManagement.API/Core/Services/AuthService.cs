using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography;
using TaskManagement.API.Core.DbContexts;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.OtherObjects;

namespace TaskManagement.API.Core.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly ITokenService _tokenService;

        public AuthService(ApplicationDbContext context, IConfiguration configuration, ITokenService tokenService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration)); 
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }

        #region SeedRolesAsync
        public async Task<RolesServiceResponse> SeedRolesAsync()
        {
            var existingRoles = await _context.Roles.ToListAsync();

            var rolesToSeed = RoleSeedData.Roles.Where(role => !existingRoles.Any(existingRole => existingRole.RoleName == role.RoleName));

            foreach (var role in rolesToSeed)
            {
                _context.Roles.AddRange(role);
            }

            await _context.SaveChangesAsync();

            return new RolesServiceResponse { IsSucceed = true, Message = "Roles seeded successfully." };
        }
        #endregion

        #region LoginAsync
        public async Task<AuthServiceResponse> LoginAsync(LoginDto loginDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(user => user.UserName == loginDto.UserName);
            if (user is null)
                return new AuthServiceResponse() { IsSucceed = false, Message = "Invalid Credentials" };

            var hashedPassword = HashPassword(loginDto.Password, Convert.FromBase64String(user.PasswordSalt));

            if (hashedPassword != user.PasswordHash)
            {
                return new AuthServiceResponse() { IsSucceed = false, Message = "Invalid Credentials" };
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
                new Claim("LastName", user.LastName),
            };

            foreach (var role in userRoles)
            {
                authClaims.Add(new Claim(ClaimTypes.Role, role));
            }

            var accessToken = _tokenService.GenerateAccessToken(authClaims);

            var refreshToken = _tokenService.GenerateRefreshToken(authClaims);

            // save refreshToken to db
            RefreshToken refreshTokenObject = new RefreshToken()
            {
                UserId = user.Id,
                Token = refreshToken,
                ExpirationDate = DateTime.Now.AddMinutes(2),
            };
            _context.RefreshTokens.Add(refreshTokenObject);
            _context.SaveChanges();

            return new AuthServiceResponse()
            {
                IsSucceed = true,
                Message = accessToken,
                RefreshToken = refreshToken
            };
        }
        #endregion

        #region RegisterAsync
        public async Task<RegisterServiceResponse> RegisterAsync(RegisterDto registerDto)
        {
            var isExistUser = await _context.Users.FirstOrDefaultAsync(user => user.UserName == registerDto.UserName);

            if (isExistUser != null)
                return new RegisterServiceResponse() { IsSucceed = false, Message = "UserName Already Exsist" };

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
                return new RegisterServiceResponse() { IsSucceed = true, Message = "User Created Successfully" };
            }
            else
            {
                return new RegisterServiceResponse() { IsSucceed = false, Message = "Failed to save user data." };
            }
           
        }

        private static string HashPassword(string password, byte[] salt)
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
        public async Task<RolesServiceResponse> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == updatePermissionDto.UserName);

            if (user is null)
                return new RolesServiceResponse() { IsSucceed = false, Message = "Invalid User name !!!" };

            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == SystemRoles.ADMIN);
            if (existingRole == null)
                return new RolesServiceResponse() { IsSucceed = false, Message = "Admin role not found." };

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            if (userRole == null)
                return new RolesServiceResponse() { IsSucceed = false, Message = "User role not found." };

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

                return new RolesServiceResponse() { IsSucceed = true, Message = "Now user is an Admin" };
            }

            return new RolesServiceResponse() { IsSucceed = false, Message = "User is already an Admin" };
        }
        #endregion

        #region MakeSuperAdminAsync
        public async Task<RolesServiceResponse> MakeSuperAdminAsync(UpdatePermissionDto updatePermissionDto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == updatePermissionDto.UserName);

            if (user is null)
                return new RolesServiceResponse() { IsSucceed = false, Message = "Invalid User name !!!" };

            var existingRole = await _context.Roles.FirstOrDefaultAsync(r => r.RoleName == SystemRoles.SUPERADMIN);
            if (existingRole == null)
                return new RolesServiceResponse() { IsSucceed = false, Message = "SuperAdmin role not found." };

            var userRole = await _context.Roles.FirstOrDefaultAsync(r => r.Id == user.RoleId);
            if (userRole == null)
                return new RolesServiceResponse() { IsSucceed = false, Message = "User role not found." };

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

                return new RolesServiceResponse() { IsSucceed = true, Message = "Now user is a SuperAdmin" };
            }

            return new RolesServiceResponse() { IsSucceed = false, Message = "User is already a SuperAdmin" };
        }
        #endregion
    }
}

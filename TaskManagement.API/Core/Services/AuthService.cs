//using Microsoft.AspNetCore.Identity;
//using Microsoft.EntityFrameworkCore;
//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using TaskManagement.API.Core.DbContexts;
//using TaskManagement.API.Core.Dtos;
//using TaskManagement.API.Core.Entities;
//using TaskManagement.API.Core.Interface;
//using TaskManagement.API.Core.OtherObjects;

//namespace TaskManagement.API.Core.Services
//{
//    public class AuthService : IAuthService
//    {
//        private readonly UserManager<ApplicationUser> _userManager;
//        private readonly RoleManager<IdentityRole> _roleManager;
//        private readonly IConfiguration _configuration;
//        private readonly ApplicationDbContext _context;
//        public AuthService(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> rolemanager, IConfiguration configuration, ApplicationDbContext context)
//        {
//            _userManager = userManager ?? throw new ArgumentNullException(nameof(userManager));
//            _roleManager = rolemanager ?? throw new ArgumentNullException(nameof(rolemanager));
//            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
//            _context = context ?? throw new ArgumentNullException(nameof(context));
//        }

//        public async Task<AuthServiceResponseDto> SeedRolesAsync()
//        {
//            bool isAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.ADMIN);
//            bool isUserRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.USER);
//            bool isSuperAdminRoleExist = await _roleManager.RoleExistsAsync(StaticUserRoles.SUPERADMIN);

//            bool isDeveloperExist = await _roleManager.RoleExistsAsync(StaticUserRoles.DEVELOPER);
//            bool isTeamLeadExist = await _roleManager.RoleExistsAsync(StaticUserRoles.TEAMLEAD);
//            bool isDesignerExist = await _roleManager.RoleExistsAsync(StaticUserRoles.DESIGNER);
//            bool isTesterExist = await _roleManager.RoleExistsAsync(StaticUserRoles.TESTER);

//            if (isAdminRoleExist && isUserRoleExist && isSuperAdminRoleExist && isDeveloperExist && isTeamLeadExist && isDesignerExist && isTesterExist)
//                return new AuthServiceResponseDto() { IsSucceed = true, Message = "Roles Seeding Is Already Done" };

//            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.USER));
//            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.ADMIN));
//            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.SUPERADMIN));
//            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.DEVELOPER));
//            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.DESIGNER));
//            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.TEAMLEAD));
//            await _roleManager.CreateAsync(new IdentityRole(StaticUserRoles.TESTER));

//            return new AuthServiceResponseDto() { IsSucceed = true, Message = "Roles Seeding Done Successfully" };
//        }


//        public async Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
//        {
//            var isExistUser = await _userManager.FindByNameAsync(registerDto.UserName);
//            if (isExistUser != null)
//                return new AuthServiceResponseDto() { IsSucceed = false, Message = "UserName Already Exsist" };

//            // AspNetUser table => IdentityUser library
//            ApplicationUser newUser = new ApplicationUser()
//            {
//                FirstName = registerDto.FirstName,
//                LastName = registerDto.LastName,
//                Email = registerDto.Email,
//                UserName = registerDto.UserName,
//                SecurityStamp = Guid.NewGuid().ToString()
//            };

//            var createUserResult = await _userManager.CreateAsync(newUser, registerDto.Password);

//            if (!createUserResult.Succeeded)
//            {
//                var errorString = "User Creation Failed Because: ";

//                foreach (var error in createUserResult.Errors)
//                {
//                    errorString += " # " + error.Description;
//                }
//                return new AuthServiceResponseDto() { IsSucceed = false, Message = errorString };
//            }

//            // Add a Default User role to all user
//            await _userManager.AddToRoleAsync(newUser, StaticUserRoles.USER);


//            // add registered user to UserEntity
//            if (createUserResult.Succeeded)
//            {
//                // Convert string UserRole to UserRole enum

//                UserEntity userEntity = new UserEntity()
//                {
//                    Id = isExistUser.Id,
//                    PasswordHash = isExistUser.PasswordHash,
//                    UserName = registerDto.UserName,
//                    FirstName = registerDto.FirstName,
//                    LastName = registerDto.LastName,
//                    Email = registerDto.Email,
//                    Role = registerDto.UserRole.ToString(),
//                    PhoneNumber = registerDto.PhoneNumber,
//                    CreatedDate = DateTime.UtcNow,
//                };

//                await _context.Users.AddAsync(userEntity);
//                if (await _context.SaveChangesAsync() >= 0)
//                {
//                    return new AuthServiceResponseDto() { IsSucceed = true, Message = "User Created Successfully" };
//                }
//                else
//                {
//                    return new AuthServiceResponseDto() { IsSucceed = false, Message = "Failed to save user data." };
//                }
//            }
//            else
//            {
//                return new AuthServiceResponseDto() { IsSucceed = false, Message = "User Not Found" };
//            }
//        }

//public async Task<AuthServiceResponseDto> LoginAsync(LoginDto loginDto)
//{
//    var user = await _userManager.FindByNameAsync(loginDto.UserName);

//    if (user is null)
//        return new AuthServiceResponseDto() { IsSucceed = false, Message = "Invalid Credentials" };

//    var isPasswordCorrect = await _userManager.CheckPasswordAsync(user, loginDto.Password);

//    if (!isPasswordCorrect)
//        return new AuthServiceResponseDto() { IsSucceed = false, Message = "Invalid Credentials" };

//    var userRoles = await _userManager.GetRolesAsync(user);

//    var authClaims = new List<Claim>
//            {
//                new Claim(ClaimTypes.Name, user.UserName),
//                new Claim(ClaimTypes.NameIdentifier, user.Id),
//                new Claim("JWTID", Guid.NewGuid().ToString()),
//                new Claim("FirstName", user.FirstName),
//                new Claim("LastName", user.LastName)
//            };

//    foreach (var userRole in userRoles)
//    {
//        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
//    }

//    var token = GenerateNewJsonWebToken(authClaims);

//    return new AuthServiceResponseDto() { IsSucceed = true, Message = token };
//}

//        private string GenerateNewJsonWebToken(List<Claim> claims)
//        {
//            var authSecret = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));

//            var tokenObject = new JwtSecurityToken(
//                    issuer: _configuration["JWT:ValidIssuer"],
//                    audience: _configuration["JWT:ValidAudience"],
//                    expires: DateTime.Now.AddHours(1),
//                    claims: claims,
//                    signingCredentials: new SigningCredentials(authSecret, SecurityAlgorithms.HmacSha256)
//                );

//            string token = new JwtSecurityTokenHandler().WriteToken(tokenObject);

//            return token;
//        }

//        public async Task<AuthServiceResponseDto> MakeAdminAsync(UpdatePermissionDto updatePermissionDto)
//        {
//            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

//            if (user is null)
//                return new AuthServiceResponseDto() { IsSucceed = false, Message = "Invalid User name !!!" };

//            await _userManager.AddToRoleAsync(user, StaticUserRoles.ADMIN);

//            return new AuthServiceResponseDto() { IsSucceed = true, Message = "Now user is an Admin" };
//        }

//        public async Task<AuthServiceResponseDto> MakeSuperAdminAsync(UpdatePermissionDto updatePermissionDto)
//        {
//            var user = await _userManager.FindByNameAsync(updatePermissionDto.UserName);

//            if (user is null)
//                return new AuthServiceResponseDto() { IsSucceed = false, Message = "Invalid User name !!!" };

//            await _userManager.AddToRoleAsync(user, StaticUserRoles.SUPERADMIN);

//            return new AuthServiceResponseDto() { IsSucceed = true, Message = "Now user is a Super Admin" };
//        }

//    }
//}
//// add test code
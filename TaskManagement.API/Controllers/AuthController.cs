using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.OtherObjects;
using TaskManagement.API.Core.Services;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService ?? throw new ArgumentNullException(nameof(authService));
        }

        [HttpPost]
        [Route("seed-roles")]
        public async Task<IActionResult> SeedRoles()
        {
            var seedRoles = await _authService.SeedRolesAsync();

            return Ok(seedRoles);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto registerDto)
        {
            var registerResult = await _authService.RegisterAsync(registerDto);

            if (registerResult.IsSucceed)
                return Ok(registerResult);

            return BadRequest(registerResult);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var loginResult = await _authService.LoginAsync(loginDto);

            if (loginResult.IsSucceed)
                return Ok(loginResult);

            return BadRequest(loginResult);
        }







        //[HttpPost("refresh-token")]
        //public async Task<IActionResult> RefreshToken([FromBody] RefreshTokenRequestDto requestDto)
        //{
        //    // Validate and retrieve the refresh token from the database
        //    var refreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(rt => rt.Token == requestDto.RefreshToken);

        //    if (refreshToken == null || refreshToken.ExpirationDate < DateTime.Now)
        //    {
        //        return Unauthorized(); // Invalid or expired refresh token
        //    }

        //    // Retrieve the user details
        //    var user = await _context.Users.FindAsync(refreshToken.UserId);

        //    // Generate new access token
        //    var authClaims = new List<Claim>
        //    {
        //        new Claim(ClaimTypes.Name, user.UserName),
        //        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        //        new Claim("JWTID", Guid.NewGuid().ToString()),
        //        // Add other claims
        //    };

        //    var newAccessToken = GenerateJsonWebToken(authClaims, authSecret, _configuration["JWT:ValidIssuer"], _configuration["JWT:ValidAudience"], TimeSpan.FromHours(1));

        //    return Ok(new
        //    {
        //        AccessToken = newAccessToken
        //    });
        //}








        // Route -> make user -> ADMIN
        [HttpPost("make-admin")]
        public async Task<IActionResult> MakeAdmin([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeAdminAsync(updatePermissionDto);
            if (operationResult.IsSucceed)
                return Ok(operationResult);

            return BadRequest(operationResult);
        }

        // Route -> make user -> SUPERADMIN
        [HttpPost("make-super-admin")]
        public async Task<IActionResult> SuperAdminAction([FromBody] UpdatePermissionDto updatePermissionDto)
        {
            var operationResult = await _authService.MakeSuperAdminAsync(updatePermissionDto);
            if (operationResult.IsSucceed)
                return Ok(operationResult);

            return BadRequest(operationResult);
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Core.DataAccess;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TokenController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITokenService _tokenService;

        public TokenController(ApplicationDbContext context, ITokenService tokenService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _tokenService = tokenService ?? throw new ArgumentNullException(nameof(tokenService));
        }


        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken(string refreshTokenString)
        {
            var newAccessToken = await _tokenService.RefreshAccessTokenAsync(refreshTokenString);

            if (newAccessToken == null)
            {
                return Unauthorized("Invalid or expired refresh token.");
            }

            return Ok(new
            {
                AccessToken = newAccessToken
            });
        }


        [HttpPost]
        [Route("revoke-token")]
        public async Task<IActionResult> RevokeToken(string refreshTokenString)
        {
            if (string.IsNullOrEmpty(refreshTokenString))
            {
                return BadRequest("Invalid token.");
            }

            var result = await _tokenService.RevokeTokenAsync(refreshTokenString);

            if (result.IsSucceed)
            {
                return Ok("Token revoked successfully.");
            }
            else
            {
                return NotFound();
            }
        }
    }
}

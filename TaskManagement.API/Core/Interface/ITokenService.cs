using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using TaskManagement.API.Core.Common;

namespace TaskManagement.API.Core.Interface
{
    public interface ITokenService
    {
        string GenerateAccessToken(IEnumerable<Claim> claims);
        string GenerateRefreshToken(IEnumerable<Claim> claims);
        Task<TokenServiceResponse> RevokeTokenAsync(string refreshTokenString);
        Task<TokenServiceResponse> RefreshAccessTokenAsync(string refreshToken);
    }
}

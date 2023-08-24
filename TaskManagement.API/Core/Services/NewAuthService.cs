using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Core.Services
{
    public class NewAuthService : IAuthService
    {
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

        public Task<AuthServiceResponseDto> RegisterAsync(RegisterDto registerDto)
        {
            throw new NotImplementedException();
        }

        public Task<AuthServiceResponseDto> SeedRolesAsync()
        {
            throw new NotImplementedException();
        }
    }
}

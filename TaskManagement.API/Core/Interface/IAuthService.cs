using TaskManagement.API.Core.Common;
using TaskManagement.API.Core.Dtos;

namespace TaskManagement.API.Core.Interface
{
    public interface IAuthService
    {
        Task<RolesServiceResponse> SeedRolesAsync();
        Task<RegisterServiceResponse> RegisterAsync(RegisterDto registerDto);
        Task<AuthServiceResponse> LoginAsync(LoginDto loginDto);
        Task<RolesServiceResponse> MakeAdminAsync(UpdatePermissionDto updatePermissionDto);
        Task<RolesServiceResponse> MakeSuperAdminAsync(UpdatePermissionDto updatePermissionDto);
    }
}

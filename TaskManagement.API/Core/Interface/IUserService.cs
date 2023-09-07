using TaskManagement.API.Core.Common;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Interface
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<List<ApplicationUser>> GetUsersByTeamId(int teamId);
        Task<UserServiceResponse> DeleteUserByIdAsync(int userId);
        Task<UserServiceResponse> UpdateUserAsync(int userId, UpdateUserDto updateUserDto);
        Task<UserServiceResponse> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
    }
}

using TaskManagement.API.Core.Dtos;

namespace TaskManagement.API.Core.Interface
{
    public interface IUserService
    {
        Task<List<UserDto>> GetAllUsersAsync();
        Task<UserDto> GetUserByIdAsync(int userId);
        Task<AuthServiceResponseDto> DeleteUserByIdAsync(int userId);
    }
}

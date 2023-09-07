using TaskManagement.API.Core.Common;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;

namespace TaskManagement.API.Core.Interface
{
    public interface ITeamService
    {
        Task<Team> CreateTeamAsync(TeamCreateDto teamDto);
        Task<IEnumerable<TeamWithUsersDto>> GetTeamsWithUsersAsync();
        Task<TeamResponse> DeleteTeamAsync(int teamId);
    }
}

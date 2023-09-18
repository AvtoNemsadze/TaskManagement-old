using Microsoft.EntityFrameworkCore;
using TaskManagement.API.Core.Common;
using TaskManagement.API.Core.DataAccess;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Entities;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Core.Services
{
    public class TeamService : ITeamService
    {
        private readonly ApplicationDbContext _context;
        private readonly IUserService _userService;
      public TeamService(ApplicationDbContext context, IUserService userService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _userService = userService ?? throw new ArgumentNullException(nameof(userService));
        }

        public async Task<Team> CreateTeamAsync(TeamCreateDto teamDto)
        {
            if (string.IsNullOrWhiteSpace(teamDto.Name))
            {
                throw new ArgumentException("Team name is required.");
            }

            // Create a new Team entity
            var newTeam = new Team
            {
                Name = teamDto.Name,
                Description = teamDto.Description,
                CreatedDate = DateTime.UtcNow,
                CreatorId = teamDto.CreatorId,
            };

            foreach (var memberId in teamDto.MemberIds)
            {
                var member = await _context.Users.FindAsync(memberId);
                if (member != null)
                {
                    newTeam.Members.Add(member);
                }
            }


            _context.Teams.Add(newTeam);
            await _context.SaveChangesAsync();

            return newTeam;
        }

        public async Task<IEnumerable<TeamWithUsersDto>> GetTeamsWithUsersAsync()
        {
            var teams = await _context.Teams
                .Include(t => t.Members) 
                .ToListAsync();

            return teams.Select(team => new TeamWithUsersDto
            {
                Id = team.Id,
                Name = team.Name,
                Description = team.Description,
                Members = team.Members.Select(user => user.UserName).ToList() 
            });
        }

        public async Task<TeamResponse> DeleteTeamAsync(int id)
        {
            await DisassociateUsersFromTeamAsync(id);
            await DisassociateTasksFromTeamAsync(id);

            var team = await _context.Teams.FindAsync(id);

            if (team == null)
            {
                return new TeamResponse() { IsSucceed = false, Message = "Team not found." };
            }

            _context.Teams.Remove(team);
            await _context.SaveChangesAsync();

            return new TeamResponse() { IsSucceed = true, Message = "Team Deleted Successfully." }; 
        }

        public async Task DisassociateUsersFromTeamAsync(int teamId)
        {
            var usersToRemoveTeam = await _userService.GetUsersByTeamId(teamId);

            foreach (var user in usersToRemoveTeam)
            {
                user.TeamId = null;
            }

            await _context.SaveChangesAsync();
        }

        public async Task DisassociateTasksFromTeamAsync(int teamId)
        {
            var tasksToUpdate = await _context.Tasks.Where(t => t.TeamId == teamId).ToListAsync();

            foreach (var task in tasksToUpdate)
            {
                task.TeamId = null;
            }

            await _context.SaveChangesAsync();
        }

    }
}

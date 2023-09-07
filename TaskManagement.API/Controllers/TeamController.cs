using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Core.DbContexts;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Enums;
using TaskManagement.API.Core.Interface;
using TaskManagement.API.Core.OtherObjects;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ApplicationDbContext _context;
        private readonly ITeamService _teamService;
        public TeamController(ApplicationDbContext context, ITeamService teamService)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        }


        [HttpPost]
        [Authorize(Roles = nameof(UserRoles.TeamLead))]
        public async Task<IActionResult> CreateTeam([FromBody] TeamCreateDto teamDto)
        {
            try
            {
                var createdTeam = await _teamService.CreateTeamAsync(teamDto);
                return Ok(createdTeam);
            }
            catch (Exception ex)
            {
                return BadRequest($"Error: {ex.Message}");
            }
        }

        [HttpGet("teams-with-members")]
        public async Task<ActionResult<IEnumerable<TeamWithUsersDto>>> GetTeamsWithUsers()
        {
            var teamsWithUsers = await _teamService.GetTeamsWithUsersAsync();
            return Ok(teamsWithUsers);
        }

        [HttpDelete("{id}")]
        [Authorize(Policy = "AdminOrSuperAdminPolicy")]
        public async Task<IActionResult> DeleteTeam(int id)
        {
            var teamToDelete = await _teamService.DeleteTeamAsync(id);

            if (teamToDelete.IsSucceed)
            {
                return Ok(teamToDelete.Message);
            }
            else
            {
                return NotFound(teamToDelete.Message);
            }
        }

    }
}

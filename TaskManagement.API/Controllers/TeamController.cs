using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManagement.API.Core.Dtos;
using TaskManagement.API.Core.Interface;

namespace TaskManagement.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TeamController : ControllerBase
    {
        private readonly ITeamService _teamService;
        public TeamController(ITeamService teamService)
        {
            _teamService = teamService ?? throw new ArgumentNullException(nameof(teamService));
        }


        [HttpPost]
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

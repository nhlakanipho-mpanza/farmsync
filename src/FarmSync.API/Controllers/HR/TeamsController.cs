using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers.HR;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TeamsController : ControllerBase
{
    private readonly ITeamService _teamService;

    public TeamsController(ITeamService teamService)
    {
        _teamService = teamService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TeamDTO>>> GetAll()
    {
        var teams = await _teamService.GetAllTeamsAsync();
        return Ok(teams);
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<TeamDTO>>> GetActive()
    {
        var teams = await _teamService.GetActiveTeamsAsync();
        return Ok(teams);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TeamDTO>> GetById(Guid id)
    {
        var team = await _teamService.GetTeamByIdAsync(id);
        if (team == null)
        {
            return NotFound($"Team with ID {id} not found.");
        }
        return Ok(team);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TeamDTO>> Create([FromBody] CreateTeamDTO dto)
    {
        var team = await _teamService.CreateTeamAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = team.Id }, team);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TeamDTO>> Update(Guid id, [FromBody] UpdateTeamDTO dto)
    {
        try
        {
            var team = await _teamService.UpdateTeamAsync(id, dto);
            return Ok(team);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _teamService.DeleteTeamAsync(id);
        return NoContent();
    }

    [HttpGet("{teamId}/members")]
    public async Task<ActionResult<IEnumerable<TeamMemberDTO>>> GetMembers(Guid teamId)
    {
        var members = await _teamService.GetTeamMembersAsync(teamId);
        return Ok(members);
    }

    [HttpPost("members")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TeamMemberDTO>> AddMember([FromBody] CreateTeamMemberDTO dto)
    {
        var member = await _teamService.AddTeamMemberAsync(dto);
        return Ok(member);
    }

    [HttpPut("members/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TeamMemberDTO>> UpdateMember(Guid id, [FromBody] UpdateTeamMemberDTO dto)
    {
        try
        {
            var member = await _teamService.UpdateTeamMemberAsync(id, dto);
            return Ok(member);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("members/{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> RemoveMember(Guid id)
    {
        await _teamService.RemoveTeamMemberAsync(id);
        return NoContent();
    }
}

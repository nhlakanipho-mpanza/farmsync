using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmSync.API.Controllers.HR;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class IssuingController : ControllerBase
{
    private readonly IIssuingService _issuingService;

    public IssuingController(IIssuingService issuingService)
    {
        _issuingService = issuingService;
    }

    // Inventory Issuing
    [HttpPost("inventory/request")]
    [Authorize(Roles = "Admin,Manager,TeamLeader")]
    public async Task<ActionResult<InventoryIssueDTO>> RequestInventory([FromBody] CreateInventoryIssueDTO dto)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        var issue = await _issuingService.RequestInventoryAsync(dto, username);
        return Ok(issue);
    }

    [HttpPost("inventory/{id}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<InventoryIssueDTO>> ApproveInventory(Guid id, [FromBody] ApproveInventoryIssueDTO dto)
    {
        try
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
            var issue = await _issuingService.ApproveInventoryIssueAsync(id, dto, username);
            return Ok(issue);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPost("inventory/{id}/return")]
    public async Task<ActionResult<InventoryIssueDTO>> ReturnInventory(Guid id, [FromBody] ReturnInventoryIssueDTO dto)
    {
        try
        {
            var issue = await _issuingService.ReturnInventoryAsync(id, dto);
            return Ok(issue);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("inventory/pending")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<IEnumerable<InventoryIssueDTO>>> GetPendingInventoryApprovals()
    {
        var issues = await _issuingService.GetPendingInventoryApprovalsAsync();
        return Ok(issues);
    }

    [HttpGet("inventory/team/{teamId}")]
    public async Task<ActionResult<IEnumerable<InventoryIssueDTO>>> GetInventoryIssuesByTeam(Guid teamId)
    {
        var issues = await _issuingService.GetInventoryIssuesByTeamAsync(teamId);
        return Ok(issues);
    }

    // Equipment Issuing
    [HttpPost("equipment/request")]
    [Authorize(Roles = "Admin,Manager,TeamLeader")]
    public async Task<ActionResult<EquipmentIssueDTO>> RequestEquipment([FromBody] CreateEquipmentIssueDTO dto)
    {
        var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
        var issue = await _issuingService.RequestEquipmentAsync(dto, username);
        return Ok(issue);
    }

    [HttpPost("equipment/{id}/approve")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<EquipmentIssueDTO>> ApproveEquipment(Guid id, [FromBody] ApproveEquipmentIssueDTO dto)
    {
        try
        {
            var username = User.FindFirst(ClaimTypes.Name)?.Value ?? "Unknown";
            var issue = await _issuingService.ApproveEquipmentIssueAsync(id, dto, username);
            return Ok(issue);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPost("equipment/{id}/return")]
    public async Task<ActionResult<EquipmentIssueDTO>> ReturnEquipment(Guid id, [FromBody] ReturnEquipmentIssueDTO dto)
    {
        try
        {
            var issue = await _issuingService.ReturnEquipmentAsync(id, dto);
            return Ok(issue);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpGet("equipment/pending")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<IEnumerable<EquipmentIssueDTO>>> GetPendingEquipmentApprovals()
    {
        var issues = await _issuingService.GetPendingEquipmentApprovalsAsync();
        return Ok(issues);
    }

    [HttpGet("equipment/team/{teamId}")]
    public async Task<ActionResult<IEnumerable<EquipmentIssueDTO>>> GetEquipmentIssuesByTeam(Guid teamId)
    {
        var issues = await _issuingService.GetEquipmentIssuesByTeamAsync(teamId);
        return Ok(issues);
    }

    [HttpGet("equipment/overdue")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<IEnumerable<EquipmentIssueDTO>>> GetOverdueReturns()
    {
        var issues = await _issuingService.GetOverdueEquipmentReturnsAsync();
        return Ok(issues);
    }
}

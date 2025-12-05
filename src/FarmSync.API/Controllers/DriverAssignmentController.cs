using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class DriverAssignmentController : ControllerBase
{
    private readonly IDriverAssignmentService _assignmentService;
    private readonly ILogger<DriverAssignmentController> _logger;

    public DriverAssignmentController(
        IDriverAssignmentService assignmentService,
        ILogger<DriverAssignmentController> logger)
    {
        _assignmentService = assignmentService;
        _logger = logger;
    }

    [HttpGet("current")]
    public async Task<ActionResult<IEnumerable<DriverAssignmentDTO>>> GetCurrentAssignments()
    {
        try
        {
            var assignments = await _assignmentService.GetCurrentAssignmentsAsync();
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current assignments");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<DriverAssignmentDTO>> GetById(Guid id)
    {
        try
        {
            var assignment = await _assignmentService.GetAssignmentByIdAsync(id);
            return Ok(assignment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assignment {AssignmentId}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<ActionResult<IEnumerable<DriverAssignmentDTO>>> GetByVehicle(Guid vehicleId)
    {
        try
        {
            var assignments = await _assignmentService.GetAssignmentsByVehicleAsync(vehicleId);
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assignments for vehicle {VehicleId}", vehicleId);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("vehicle/{vehicleId}/current")]
    public async Task<ActionResult<DriverAssignmentDTO>> GetCurrentByVehicle(Guid vehicleId)
    {
        try
        {
            var assignment = await _assignmentService.GetCurrentAssignmentByVehicleAsync(vehicleId);
            if (assignment == null)
                return NotFound($"No current assignment found for vehicle {vehicleId}");
            
            return Ok(assignment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting current assignment for vehicle {VehicleId}", vehicleId);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("driver/{driverId}")]
    public async Task<ActionResult<IEnumerable<DriverAssignmentDTO>>> GetByDriver(Guid driverId)
    {
        try
        {
            var assignments = await _assignmentService.GetAssignmentsByDriverAsync(driverId);
            return Ok(assignments);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting assignments for driver {DriverId}", driverId);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<DriverAssignmentDTO>> AssignDriver([FromBody] CreateDriverAssignmentDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var assignedById))
            {
                return Unauthorized("User ID not found in token");
            }

            var assignment = await _assignmentService.AssignDriverToVehicleAsync(dto, assignedById);
            return CreatedAtAction(nameof(GetById), new { id = assignment.Id }, assignment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error assigning driver to vehicle");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<DriverAssignmentDTO>> Update(Guid id, [FromBody] UpdateDriverAssignmentDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var assignment = await _assignmentService.UpdateAssignmentAsync(id, dto);
            return Ok(assignment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating assignment {AssignmentId}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost("{id}/end")]
    public async Task<ActionResult> EndAssignment(Guid id, [FromBody] DateTime endDate)
    {
        try
        {
            await _assignmentService.EndAssignmentAsync(id, endDate);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error ending assignment {AssignmentId}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _assignmentService.DeleteAssignmentAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting assignment {AssignmentId}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

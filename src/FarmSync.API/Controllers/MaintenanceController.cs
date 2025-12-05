using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IMaintenanceService _maintenanceService;

    public MaintenanceController(IMaintenanceService maintenanceService)
    {
        _maintenanceService = maintenanceService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<MaintenanceRecordDTO>>> GetAll()
    {
        try
        {
            var records = await _maintenanceService.GetAllMaintenanceAsync();
            return Ok(records);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<MaintenanceRecordDTO>> GetById(Guid id)
    {
        try
        {
            var record = await _maintenanceService.GetMaintenanceByIdAsync(id);
            return Ok(record);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("vehicle/{vehicleId}")]
    public async Task<ActionResult<IEnumerable<MaintenanceRecordDTO>>> GetByVehicle(Guid vehicleId)
    {
        try
        {
            var records = await _maintenanceService.GetByVehicleAsync(vehicleId);
            return Ok(records);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("overdue")]
    public async Task<ActionResult<IEnumerable<MaintenanceRecordDTO>>> GetOverdue()
    {
        try
        {
            var records = await _maintenanceService.GetPendingMaintenanceAsync();
            return Ok(records);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // TODO: Implement GetByMaintenanceTypeAsync in IMaintenanceService
    // [HttpGet("type/{typeId}")]
    // public async Task<ActionResult<IEnumerable<MaintenanceRecordDTO>>> GetByType(Guid typeId)
    // {
    //     try
    //     {
    //         var records = await _maintenanceService.GetMaintenanceByTypeAsync(typeId);
    //         return Ok(records);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"Internal server error: {ex.Message}");
    //     }
    // }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<MaintenanceRecordDTO>>> GetByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var records = await _maintenanceService.GetCompletedMaintenanceAsync(startDate, endDate);
            return Ok(records);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<MaintenanceRecordDTO>> Create([FromBody] CreateMaintenanceRecordDTO dto)
    {
        try
        {
            var record = await _maintenanceService.CreateMaintenanceAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = record.Id }, record);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<MaintenanceRecordDTO>> Update(Guid id, [FromBody] UpdateMaintenanceRecordDTO dto)
    {
        try
        {
            var record = await _maintenanceService.UpdateMaintenanceAsync(id, dto);
            return Ok(record);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _maintenanceService.DeleteMaintenanceAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

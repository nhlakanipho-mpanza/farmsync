using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class FuelController : ControllerBase
{
    private readonly IFuelService _fuelService;

    public FuelController(IFuelService fuelService)
    {
        _fuelService = fuelService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<FuelLogDTO>>> GetAll()
    {
        try
        {
            var logs = await _fuelService.GetAllFuelLogsAsync();
            return Ok(logs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<FuelLogDTO>> GetById(Guid id)
    {
        try
        {
            var log = await _fuelService.GetFuelLogByIdAsync(id);
            return Ok(log);
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
    public async Task<ActionResult<IEnumerable<FuelLogDTO>>> GetByVehicle(Guid vehicleId)
    {
        try
        {
            var logs = await _fuelService.GetByVehicleAsync(vehicleId);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // TODO: Implement GetRecentFuelLogsAsync in IFuelService
    // [HttpGet("recent")]
    // public async Task<ActionResult<IEnumerable<FuelLogDTO>>> GetRecent([FromQuery] int count = 10)
    // {
    //     try
    //     {
    //         var logs = await _fuelService.GetRecentFuelLogsAsync(count);
    //         return Ok(logs);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"Internal server error: {ex.Message}");
    //     }
    // }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<FuelLogDTO>>> GetByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var logs = await _fuelService.GetByDateRangeAsync(startDate, endDate);
            return Ok(logs);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("vehicle/{vehicleId}/efficiency")]
    public async Task<ActionResult<decimal?>> GetAverageFuelEfficiency(Guid vehicleId)
    {
        try
        {
            var efficiency = await _fuelService.GetAverageFuelConsumptionAsync(vehicleId);
            return Ok(new { vehicleId, averageFuelEfficiency = efficiency });
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<FuelLogDTO>> Create([FromBody] CreateFuelLogDTO dto)
    {
        try
        {
            var log = await _fuelService.CreateFuelLogAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = log.Id }, log);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<FuelLogDTO>> Update(Guid id, [FromBody] UpdateFuelLogDTO dto)
    {
        try
        {
            var log = await _fuelService.UpdateFuelLogAsync(id, dto);
            return Ok(log);
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
            await _fuelService.DeleteFuelLogAsync(id);
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

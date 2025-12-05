using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TripLogsController : ControllerBase
{
    private readonly ITripLogService _tripLogService;

    public TripLogsController(ITripLogService tripLogService)
    {
        _tripLogService = tripLogService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TripLogDTO>>> GetAll()
    {
        try
        {
            var trips = await _tripLogService.GetAllTripsAsync();
            return Ok(trips);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TripLogDTO>> GetById(Guid id)
    {
        try
        {
            var trip = await _tripLogService.GetTripByIdAsync(id);
            return Ok(trip);
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
    public async Task<ActionResult<IEnumerable<TripLogDTO>>> GetByVehicle(Guid vehicleId)
    {
        try
        {
            var trips = await _tripLogService.GetByVehicleAsync(vehicleId);
            return Ok(trips);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("driver/{driverId}")]
    public async Task<ActionResult<IEnumerable<TripLogDTO>>> GetByDriver(Guid driverId)
    {
        try
        {
            var trips = await _tripLogService.GetByDriverAsync(driverId);
            return Ok(trips);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // TODO: Implement GetActiveTripsAsync() in ITripLogService
    // [HttpGet("active")]
    // public async Task<ActionResult<IEnumerable<TripLogDTO>>> GetActiveTrips()
    // {
    //     try
    //     {
    //         var trips = await _tripLogService.GetActiveTripsAsync();
    //         return Ok(trips);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"Internal server error: {ex.Message}");
    //     }
    // }

    [HttpGet("date-range")]
    public async Task<ActionResult<IEnumerable<TripLogDTO>>> GetByDateRange(
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var trips = await _tripLogService.GetByDateRangeAsync(startDate, endDate);
            return Ok(trips);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TripLogDTO>> Create([FromBody] CreateTripLogDTO dto)
    {
        try
        {
            var trip = await _tripLogService.CreateTripAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = trip.Id }, trip);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TripLogDTO>> Update(Guid id, [FromBody] UpdateTripLogDTO dto)
    {
        try
        {
            var trip = await _tripLogService.UpdateTripAsync(id, dto);
            return Ok(trip);
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
            await _tripLogService.DeleteTripAsync(id);
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

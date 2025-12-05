using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class GPSController : ControllerBase
{
    private readonly IGPSTrackingService _gpsService;

    public GPSController(IGPSTrackingService gpsService)
    {
        _gpsService = gpsService;
    }

    [HttpGet("active-vehicles")]
    public async Task<ActionResult<IEnumerable<GPSLocationDTO>>> GetActiveVehicleLocations()
    {
        try
        {
            var locations = await _gpsService.GetActiveVehicleLocationsAsync();
            return Ok(locations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("vehicle/{vehicleId}/latest")]
    public async Task<ActionResult<GPSLocationDTO>> GetLatestLocation(Guid vehicleId)
    {
        try
        {
            var location = await _gpsService.GetLatestLocationAsync(vehicleId);
            return Ok(location);
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

    [HttpGet("vehicle/{vehicleId}/history")]
    public async Task<ActionResult<IEnumerable<GPSLocationDTO>>> GetLocationHistory(
        Guid vehicleId,
        [FromQuery] DateTime startDate,
        [FromQuery] DateTime endDate)
    {
        try
        {
            var locations = await _gpsService.GetLocationHistoryAsync(vehicleId, startDate, endDate);
            return Ok(locations);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    // TODO: Implement GetVehiclesInGeofenceAsync in IGPSTrackingService
    // [HttpGet("geofence")]
    // public async Task<ActionResult<IEnumerable<GPSLocationDTO>>> GetVehiclesInGeofence(
    //     [FromQuery] double latitude,
    //     [FromQuery] double longitude,
    //     [FromQuery] double radiusKm)
    // {
    //     try
    //     {
    //         var locations = await _gpsService.GetVehiclesInGeofenceAsync(latitude, longitude, radiusKm);
    //         return Ok(locations);
    //     }
    //     catch (Exception ex)
    //     {
    //         return StatusCode(500, $"Internal server error: {ex.Message}");
    //     }
    // }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<GPSLocationDTO>> RecordLocation([FromBody] CreateGPSLocationDTO dto)
    {
        try
        {
            var location = await _gpsService.RecordLocationAsync(dto);
            return CreatedAtAction(nameof(GetLatestLocation), new { vehicleId = location.VehicleId }, location);
        }
        catch (Exception ex)
        {
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

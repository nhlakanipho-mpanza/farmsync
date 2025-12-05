using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehicleController : ControllerBase
{
    private readonly IVehicleService _vehicleService;
    private readonly ILogger<VehicleController> _logger;

    public VehicleController(IVehicleService vehicleService, ILogger<VehicleController> logger)
    {
        _vehicleService = vehicleService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetAll()
    {
        try
        {
            var vehicles = await _vehicleService.GetAllVehiclesAsync();
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all vehicles");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDTO>> GetById(Guid id)
    {
        try
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            return Ok(vehicle);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicle {VehicleId}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetActive()
    {
        try
        {
            var vehicles = await _vehicleService.GetActiveVehiclesAsync();
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active vehicles");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("registration/{registrationNumber}")]
    public async Task<ActionResult<VehicleDTO>> GetByRegistrationNumber(string registrationNumber)
    {
        try
        {
            var vehicle = await _vehicleService.GetByRegistrationNumberAsync(registrationNumber);
            return Ok(vehicle);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicle with registration {RegistrationNumber}", registrationNumber);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("type/{typeId}")]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetByType(Guid typeId)
    {
        try
        {
            var vehicles = await _vehicleService.GetByTypeAsync(typeId);
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicles by type {TypeId}", typeId);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("status/{statusId}")]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetByStatus(Guid statusId)
    {
        try
        {
            var vehicles = await _vehicleService.GetByStatusAsync(statusId);
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicles by status {StatusId}", statusId);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("maintenance/due")]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetDueForMaintenance()
    {
        try
        {
            var vehicles = await _vehicleService.GetVehiclesDueForMaintenanceAsync();
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicles due for maintenance");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpGet("license-renewal")]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetNeedingLicenseRenewal([FromQuery] int daysThreshold = 30)
    {
        try
        {
            var vehicles = await _vehicleService.GetVehiclesNeedingLicenseRenewalAsync(daysThreshold);
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicles needing license renewal");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPost]
    public async Task<ActionResult<VehicleDTO>> Create([FromBody] CreateVehicleDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _vehicleService.CreateVehicleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vehicle");
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<VehicleDTO>> Update(Guid id, [FromBody] UpdateVehicleDTO dto)
    {
        try
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var vehicle = await _vehicleService.UpdateVehicleAsync(id, dto);
            return Ok(vehicle);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vehicle {VehicleId}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _vehicleService.DeleteVehicleAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting vehicle {VehicleId}", id);
            return StatusCode(500, $"Internal server error: {ex.Message}");
        }
    }
}

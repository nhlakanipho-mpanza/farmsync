using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;

namespace FarmSync.API.Controllers.Fleet;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class VehiclesController : ControllerBase
{
    private readonly IVehicleService _vehicleService;

    public VehiclesController(IVehicleService vehicleService)
    {
        _vehicleService = vehicleService;
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
            return StatusCode(500, new { message = "An error occurred while retrieving vehicles", error = ex.Message });
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
            return StatusCode(500, new { message = "An error occurred while retrieving active vehicles", error = ex.Message });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<VehicleDTO>> GetById(Guid id)
    {
        try
        {
            var vehicle = await _vehicleService.GetVehicleByIdAsync(id);
            if (vehicle == null)
            {
                return NotFound(new { message = $"Vehicle with ID {id} not found" });
            }
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the vehicle", error = ex.Message });
        }
    }

    [HttpGet("registration/{registrationNumber}")]
    public async Task<ActionResult<VehicleDTO>> GetByRegistration(string registrationNumber)
    {
        try
        {
            var vehicle = await _vehicleService.GetByRegistrationNumberAsync(registrationNumber);
            if (vehicle == null)
            {
                return NotFound(new { message = $"Vehicle with registration {registrationNumber} not found" });
            }
            return Ok(vehicle);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving the vehicle", error = ex.Message });
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
            return StatusCode(500, new { message = "An error occurred while retrieving vehicles", error = ex.Message });
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
            return StatusCode(500, new { message = "An error occurred while retrieving vehicles", error = ex.Message });
        }
    }

    [HttpGet("driver/{driverId}")]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetByDriver(Guid driverId)
    {
        try
        {
            var vehicles = await _vehicleService.GetByDriverAsync(driverId);
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving vehicles", error = ex.Message });
        }
    }

    [HttpGet("maintenance-due")]
    public async Task<ActionResult<IEnumerable<VehicleDTO>>> GetDueForMaintenance()
    {
        try
        {
            var vehicles = await _vehicleService.GetVehiclesDueForMaintenanceAsync();
            return Ok(vehicles);
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while retrieving vehicles", error = ex.Message });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<VehicleDTO>> Create([FromBody] CreateVehicleDTO dto)
    {
        try
        {
            var vehicle = await _vehicleService.CreateVehicleAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = vehicle.Id }, vehicle);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while creating the vehicle", error = ex.Message });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<VehicleDTO>> Update(Guid id, [FromBody] UpdateVehicleDTO dto)
    {
        try
        {
            var vehicle = await _vehicleService.UpdateVehicleAsync(id, dto);
            return Ok(vehicle);
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while updating the vehicle", error = ex.Message });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _vehicleService.DeleteVehicleAsync(id);
            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "An error occurred while deleting the vehicle", error = ex.Message });
        }
    }
}

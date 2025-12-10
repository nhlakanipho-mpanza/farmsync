using FarmSync.Application.DTOs.Inventory;
using FarmSync.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class EquipmentController : ControllerBase
{
    private readonly IEquipmentService _service;
    private readonly ILogger<EquipmentController> _logger;

    public EquipmentController(IEquipmentService service, ILogger<EquipmentController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Operations Manager,Operations Clerk,Accountant,Accounting Manager")]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var equipment = await _service.GetAllAsync();
            return Ok(equipment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all equipment");
            return StatusCode(500, new { message = "An error occurred while fetching equipment" });
        }
    }

    [HttpGet("active")]
    [Authorize(Roles = "Admin,Operations Manager,Operations Clerk,Accountant,Accounting Manager")]
    public async Task<IActionResult> GetActive()
    {
        try
        {
            var equipment = await _service.GetActiveAsync();
            return Ok(equipment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting active equipment");
            return StatusCode(500, new { message = "An error occurred while fetching active equipment" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var equipment = await _service.GetByIdAsync(id);
            if (equipment == null)
                return NotFound(new { message = $"Equipment with ID {id} not found" });

            return Ok(equipment);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting equipment {Id}", id);
            return StatusCode(500, new { message = "An error occurred while fetching the equipment" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Operations Manager")]
    public async Task<IActionResult> Create([FromBody] CreateEquipmentDto dto)
    {
        try
        {
            var equipment = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = equipment.Id }, equipment);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating equipment");
            return StatusCode(500, new { message = "An error occurred while creating the equipment" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Operations Manager")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateEquipmentDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            var equipment = await _service.UpdateAsync(id, dto);
            return Ok(equipment);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating equipment {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the equipment" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting equipment {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the equipment" });
        }
    }
}

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using FarmSync.Application.DTOs.Inventory;
using FarmSync.Application.Interfaces;

namespace FarmSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class InventoryItemsController : ControllerBase
{
    private readonly IInventoryItemService _service;
    private readonly ILogger<InventoryItemsController> _logger;

    public InventoryItemsController(IInventoryItemService service, ILogger<InventoryItemsController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var items = await _service.GetAllAsync();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all inventory items");
            return StatusCode(500, new { message = "An error occurred while fetching inventory items" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
                return NotFound(new { message = $"Inventory item with ID {id} not found" });

            return Ok(item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting inventory item {Id}", id);
            return StatusCode(500, new { message = "An error occurred while fetching the inventory item" });
        }
    }

    [HttpGet("low-stock")]
    public async Task<IActionResult> GetLowStock()
    {
        try
        {
            var items = await _service.GetLowStockItemsAsync();
            return Ok(items);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting low stock items");
            return StatusCode(500, new { message = "An error occurred while fetching low stock items" });
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateInventoryItemDto dto)
    {
        try
        {
            var item = await _service.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = item.Id }, item);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating inventory item");
            return StatusCode(500, new { message = "An error occurred while creating the inventory item" });
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdateInventoryItemDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            var item = await _service.UpdateAsync(dto);
            return Ok(item);
        }
        catch (KeyNotFoundException)
        {
            return NotFound(new { message = $"Inventory item with ID {id} not found" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating inventory item {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the inventory item" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting inventory item {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the inventory item" });
        }
    }
}

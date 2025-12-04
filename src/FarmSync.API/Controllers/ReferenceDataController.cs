using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmSync.Infrastructure.Data;

namespace FarmSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ReferenceDataController : ControllerBase
{
    private readonly FarmSyncDbContext _context;
    private readonly ILogger<ReferenceDataController> _logger;

    public ReferenceDataController(FarmSyncDbContext context, ILogger<ReferenceDataController> logger)
    {
        _context = context;
        _logger = logger;
    }

    [HttpGet("categories")]
    public async Task<IActionResult> GetCategories()
    {
        try
        {
            var categories = await _context.InventoryCategories
                .Where(c => c.IsActive)
                .Select(c => new { c.Id, c.Name, c.Description })
                .ToListAsync();
            return Ok(categories);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting categories");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("types")]
    public async Task<IActionResult> GetTypes()
    {
        try
        {
            var types = await _context.InventoryTypes
                .Where(t => t.IsActive)
                .Select(t => new { t.Id, t.Name, t.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("units")]
    public async Task<IActionResult> GetUnitsOfMeasure()
    {
        try
        {
            var units = await _context.UnitsOfMeasure
                .Where(u => u.IsActive)
                .Select(u => new { u.Id, u.Name, u.Abbreviation, u.Description })
                .ToListAsync();
            return Ok(units);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting units of measure");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("statuses")]
    public async Task<IActionResult> GetTransactionStatuses()
    {
        try
        {
            var statuses = await _context.TransactionStatuses
                .Where(s => s.IsActive)
                .Select(s => new { s.Id, s.Name, s.Description })
                .ToListAsync();
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting statuses");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("conditions")]
    public async Task<IActionResult> GetEquipmentConditions()
    {
        try
        {
            var conditions = await _context.EquipmentConditions
                .Where(c => c.IsActive)
                .Select(c => new { c.Id, c.Name, c.Description })
                .ToListAsync();
            return Ok(conditions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting conditions");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("maintenance-types")]
    public async Task<IActionResult> GetMaintenanceTypes()
    {
        try
        {
            var types = await _context.MaintenanceTypes
                .Where(t => t.IsActive)
                .Select(t => new { t.Id, t.Name, t.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting maintenance types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("locations")]
    public async Task<IActionResult> GetLocations()
    {
        try
        {
            var locations = await _context.InventoryLocations
                .Where(l => l.IsActive)
                .Select(l => new { l.Id, l.Name, l.Description, l.Address })
                .ToListAsync();
            return Ok(locations);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting locations");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("all")]
    public async Task<IActionResult> GetAllReferenceData()
    {
        try
        {
            var data = new
            {
                Categories = await _context.InventoryCategories
                    .Where(c => c.IsActive)
                    .Select(c => new { c.Id, c.Name, c.Description })
                    .ToListAsync(),
                Types = await _context.InventoryTypes
                    .Where(t => t.IsActive)
                    .Select(t => new { t.Id, t.Name, t.Description })
                    .ToListAsync(),
                UnitsOfMeasure = await _context.UnitsOfMeasure
                    .Where(u => u.IsActive)
                    .Select(u => new { u.Id, u.Name, u.Abbreviation, u.Description })
                    .ToListAsync(),
                TransactionStatuses = await _context.TransactionStatuses
                    .Where(s => s.IsActive)
                    .Select(s => new { s.Id, s.Name, s.Description })
                    .ToListAsync(),
                EquipmentConditions = await _context.EquipmentConditions
                    .Where(c => c.IsActive)
                    .Select(c => new { c.Id, c.Name, c.Description })
                    .ToListAsync(),
                MaintenanceTypes = await _context.MaintenanceTypes
                    .Where(t => t.IsActive)
                    .Select(t => new { t.Id, t.Name, t.Description })
                    .ToListAsync(),
                Locations = await _context.InventoryLocations
                    .Where(l => l.IsActive)
                    .Select(l => new { l.Id, l.Name, l.Description, l.Address })
                    .ToListAsync()
            };

            return Ok(data);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all reference data");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}

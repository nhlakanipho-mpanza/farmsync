using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmSync.Infrastructure.Data;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Entities.Fleet;

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

    // HR Reference Data Endpoints
    [HttpGet("positions")]
    public async Task<IActionResult> GetPositions()
    {
        try
        {
            var positions = await _context.Positions
                .Where(p => p.IsActive)
                .Select(p => new { p.Id, p.Name, p.Rate, p.Description })
                .ToListAsync();
            return Ok(positions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting positions");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("employment-types")]
    public async Task<IActionResult> GetEmploymentTypes()
    {
        try
        {
            var types = await _context.EmploymentTypes
                .Where(t => t.IsActive)
                .Select(t => new { t.Id, t.Name, t.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employment types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("role-types")]
    public async Task<IActionResult> GetRoleTypes()
    {
        try
        {
            var types = await _context.RoleTypes
                .Where(t => t.IsActive)
                .Select(t => new { t.Id, t.Name, t.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting role types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("team-types")]
    public async Task<IActionResult> GetTeamTypes()
    {
        try
        {
            var types = await _context.TeamTypes
                .Where(t => t.IsActive)
                .Select(t => new { t.Id, t.Name, t.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting team types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("work-areas")]
    public async Task<IActionResult> GetWorkAreas()
    {
        try
        {
            var areas = await _context.WorkAreas
                .Where(a => a.IsActive)
                .Select(a => new { a.Id, a.Name, a.Location, a.Size, a.SizeUnit, a.Description })
                .ToListAsync();
            return Ok(areas);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting work areas");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("bank-names")]
    public async Task<IActionResult> GetBankNames()
    {
        try
        {
            var banks = await _context.BankNames
                .Where(b => b.IsActive)
                .Select(b => new { b.Id, b.Name, b.BranchCode })
                .ToListAsync();
            return Ok(banks);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bank names");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("account-types")]
    public async Task<IActionResult> GetAccountTypes()
    {
        try
        {
            var types = await _context.AccountTypes
                .Where(t => t.IsActive)
                .Select(t => new { t.Id, t.Name, t.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting account types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("task-statuses")]
    public async Task<IActionResult> GetTaskStatuses()
    {
        try
        {
            var statuses = await _context.TaskStatuses
                .Where(s => s.IsActive)
                .Select(s => new { s.Id, s.Name, s.ColorCode, s.Description })
                .ToListAsync();
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task statuses");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("issue-statuses")]
    public async Task<IActionResult> GetIssueStatuses()
    {
        try
        {
            var statuses = await _context.IssueStatuses
                .Where(s => s.IsActive)
                .Select(s => new { s.Id, s.Name, s.ColorCode, s.Description })
                .ToListAsync();
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting issue statuses");
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
                    .ToListAsync(),
                // HR Reference Data
                Positions = await _context.Positions
                    .Where(p => p.IsActive)
                    .Select(p => new { p.Id, p.Name, p.Rate, p.Description })
                    .ToListAsync(),
                EmploymentTypes = await _context.EmploymentTypes
                    .Where(t => t.IsActive)
                    .Select(t => new { t.Id, t.Name, t.Description })
                    .ToListAsync(),
                RoleTypes = await _context.RoleTypes
                    .Where(t => t.IsActive)
                    .Select(t => new { t.Id, t.Name, t.Description })
                    .ToListAsync(),
                TeamTypes = await _context.TeamTypes
                    .Where(t => t.IsActive)
                    .Select(t => new { t.Id, t.Name, t.Description })
                    .ToListAsync(),
                WorkAreas = await _context.WorkAreas
                    .Where(a => a.IsActive)
                    .Select(a => new { a.Id, a.Name, a.Location, a.Size, a.SizeUnit, a.Description })
                    .ToListAsync(),
                BankNames = await _context.BankNames
                    .Where(b => b.IsActive)
                    .Select(b => new { b.Id, b.Name, b.BranchCode })
                    .ToListAsync(),
                AccountTypes = await _context.AccountTypes
                    .Where(t => t.IsActive)
                    .Select(t => new { t.Id, t.Name, t.Description })
                    .ToListAsync(),
                TaskStatuses = await _context.TaskStatuses
                    .Where(s => s.IsActive)
                    .Select(s => new { s.Id, s.Name, s.ColorCode, s.Description })
                    .ToListAsync(),
                IssueStatuses = await _context.IssueStatuses
                    .Where(s => s.IsActive)
                    .Select(s => new { s.Id, s.Name, s.ColorCode, s.Description })
                    .ToListAsync(),

                // Fleet Reference Data
                VehicleTypes = await _context.VehicleTypes
                    .Where(v => v.IsActive)
                    .Select(v => new { v.Id, v.Name, v.Description })
                    .ToListAsync(),
                VehicleStatuses = await _context.VehicleStatuses
                    .Where(v => v.IsActive)
                    .Select(v => new { v.Id, v.Name, v.Description })
                    .ToListAsync(),
                FuelTypes = await _context.FuelTypes
                    .Where(f => f.IsActive)
                    .Select(f => new { f.Id, f.Name, f.Description })
                    .ToListAsync(),
                FleetMaintenanceTypes = await _context.FleetMaintenanceTypes
                    .Where(m => m.IsActive)
                    .Select(m => new { m.Id, m.Name, m.Description })
                    .ToListAsync(),
                FleetTaskStatuses = await _context.FleetTaskStatuses
                    .Where(t => t.IsActive)
                    .Select(t => new { t.Id, t.Name, t.Description })
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

    // Fleet-specific reference data endpoints
    [HttpGet("vehicle-types")]
    public async Task<IActionResult> GetVehicleTypes()
    {
        try
        {
            var types = await _context.VehicleTypes
                .Where(v => v.IsActive)
                .Select(v => new { v.Id, v.Name, v.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicle types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("vehicle-statuses")]
    public async Task<IActionResult> GetVehicleStatuses()
    {
        try
        {
            var statuses = await _context.VehicleStatuses
                .Where(v => v.IsActive)
                .Select(v => new { v.Id, v.Name, v.Description })
                .ToListAsync();
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicle statuses");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("fuel-types")]
    public async Task<IActionResult> GetFuelTypes()
    {
        try
        {
            var types = await _context.FuelTypes
                .Where(f => f.IsActive)
                .Select(f => new { f.Id, f.Name, f.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fuel types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("fleet-maintenance-types")]
    public async Task<IActionResult> GetFleetMaintenanceTypes()
    {
        try
        {
            var types = await _context.FleetMaintenanceTypes
                .Where(m => m.IsActive)
                    .Select(m => new { m.Id, m.Name, m.Description })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fleet maintenance types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("fleet-task-statuses")]
    public async Task<IActionResult> GetFleetTaskStatuses()
    {
        try
        {
            var statuses = await _context.FleetTaskStatuses
                .Where(t => t.IsActive)
                .Select(t => new { t.Id, t.Name, t.Description })
                .ToListAsync();
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fleet task statuses");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}

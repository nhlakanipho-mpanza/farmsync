using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using FarmSync.Infrastructure.Data;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Entities.ReferenceData;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Application.DTOs.ReferenceData;
using FarmSync.Application.DTOs.Inventory;

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
                .Select(c => new { c.Id, c.Name, c.Description, c.IsActive })
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
                .Select(t => new { t.Id, t.Name, t.Description, t.IsActive })
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
                .Select(u => new { u.Id, u.Name, u.Abbreviation, u.Description, u.IsActive })
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
                .Select(s => new { s.Id, s.Name, s.Description, s.IsActive })
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
                .Select(c => new { c.Id, c.Name, c.Description, c.IsActive })
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
                .Select(t => new { t.Id, t.Name, t.Description, t.IsActive })
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
                .Select(l => new { l.Id, l.Name, l.Description, l.Address, l.IsActive })
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
                .Select(p => new { p.Id, p.Name, Rate = p.Rate, p.Description, p.IsActive, p.IsDriverPosition })
                .ToListAsync();
            return Ok(positions);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting positions");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("roles")]
    public async Task<IActionResult> GetRoles()
    {
        try
        {
            var roles = await _context.Roles
                .Where(r => r.IsActive)
                .Select(r => new { r.Id, r.Name, r.Description })
                .ToListAsync();
            return Ok(roles);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting roles");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("employment-types")]
    public async Task<IActionResult> GetEmploymentTypes()
    {
        try
        {
            var types = await _context.EmploymentTypes
                .Select(t => new { t.Id, t.Name, t.Description, t.IsActive })
                .ToListAsync();
            return Ok(types);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employment types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("team-types")]
    public async Task<IActionResult> GetTeamTypes()
    {
        try
        {
            var types = await _context.TeamTypes
                .Select(t => new { t.Id, t.Name, t.Description, t.IsActive })
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
                .Select(a => new { a.Id, a.Name, a.Location, a.Size, a.SizeUnit, a.Description, a.IsActive })
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
                .Select(b => new { b.Id, b.Name, b.BranchCode, b.Description, b.IsActive })
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
                .Select(t => new { t.Id, t.Name, t.Description, t.IsActive })
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
                .Select(s => new { s.Id, s.Name, s.ColorCode, s.Description, s.IsActive })
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
                .Select(s => new { s.Id, s.Name, s.ColorCode, s.Description, s.IsActive })
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
                .Select(v => new { v.Id, v.Name, v.Description, v.IsActive })
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
                .Select(v => new { v.Id, v.Name, v.Description, v.IsActive })
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
                .Select(f => new { f.Id, f.Name, f.Description, f.IsActive })
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
                    .Select(m => new { m.Id, m.Name, m.Description, m.IsActive })
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
                .Select(t => new { t.Id, t.Name, t.Description, t.IsActive })
                .ToListAsync();
            return Ok(statuses);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fleet task statuses");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("document-types")]
    public async Task<IActionResult> GetDocumentTypes()
    {
        try
        {
            var documentTypes = await _context.DocumentTypes
                .Select(t => new { t.Id, t.Name, t.Description, t.IsActive })
                .ToListAsync();
            return Ok(documentTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting document types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("leave-types")]
    public async Task<IActionResult> GetLeaveTypes()
    {
        try
        {
            var leaveTypes = await _context.LeaveTypes
                .Select(t => new { t.Id, t.Name, t.Description, t.RequiresDocument, t.DefaultDaysPerYear, t.IsActive })
                .ToListAsync();
            return Ok(leaveTypes);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting leave types");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("field-phases")]
    public async Task<IActionResult> GetFieldPhases()
    {
        try
        {
            var fieldPhases = await _context.FieldPhases
                .OrderBy(p => p.SortOrder)
                .Select(p => new { p.Id, p.Name, p.Description, p.SortOrder, p.IsActive })
                .ToListAsync();
            return Ok(fieldPhases);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting field phases");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== CREATE ENDPOINTS ==========

    [HttpPost("bank-names")]
    public async Task<IActionResult> CreateBankName([FromBody] CreateBankNameDto dto)
    {
        try
        {
            var entity = new BankName
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                BranchCode = dto.BranchCode,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.BankNames.Add(entity);
            await _context.SaveChangesAsync();

            var result = new BankNameDto
            {
                Id = entity.Id,
                Name = entity.Name,
                BranchCode = entity.BranchCode,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetBankNames), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating bank name");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("account-types")]
    public async Task<IActionResult> CreateAccountType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new AccountType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.AccountTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetAccountTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating account type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("units")]
    public async Task<IActionResult> CreateUnitOfMeasure([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new UnitOfMeasure
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.UnitsOfMeasure.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetUnitsOfMeasure), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating unit of measure");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("employment-types")]
    public async Task<IActionResult> CreateEmploymentType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new EmploymentType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.EmploymentTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetEmploymentTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating employment type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("positions")]
    public async Task<IActionResult> CreatePosition([FromBody] CreatePositionDto dto)
    {
        try
        {
            var entity = new Position
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Rate = dto.Rate,
                Description = dto.Description,
                IsDriverPosition = dto.IsDriverPosition,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.Positions.Add(entity);
            await _context.SaveChangesAsync();

            var result = new PositionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Rate = entity.Rate,
                Description = entity.Description,
                IsDriverPosition = entity.IsDriverPosition,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetPositions), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating position");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("team-types")]
    public async Task<IActionResult> CreateTeamType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new TeamType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.TeamTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetTeamTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating team type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("work-areas")]
    public async Task<IActionResult> CreateWorkArea([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new WorkArea
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.WorkAreas.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetWorkAreas), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating work area");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("leave-types")]
    public async Task<IActionResult> CreateLeaveType([FromBody] CreateLeaveTypeDto dto)
    {
        try
        {
            var entity = new LeaveType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                RequiresDocument = dto.RequiresDocument,
                DefaultDaysPerYear = dto.DefaultDaysPerYear,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.LeaveTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new LeaveTypeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                RequiresDocument = entity.RequiresDocument,
                DefaultDaysPerYear = entity.DefaultDaysPerYear,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetLeaveTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating leave type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("document-types")]
    public async Task<IActionResult> CreateDocumentType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new DocumentType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.DocumentTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetDocumentTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating document type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("field-phases")]
    public async Task<IActionResult> CreateFieldPhase([FromBody] CreateFieldPhaseDto dto)
    {
        try
        {
            var entity = new FieldPhase
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                SortOrder = dto.SortOrder,
                IsActive = dto.IsActive,
                CreatedAt = DateTime.UtcNow
            };

            _context.FieldPhases.Add(entity);
            await _context.SaveChangesAsync();

            var result = new FieldPhaseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                SortOrder = entity.SortOrder,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetFieldPhases), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating field phase");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== UPDATE ENDPOINTS ==========

    [HttpPut("bank-names/{id:guid}")]
    public async Task<IActionResult> UpdateBankName(Guid id, [FromBody] UpdateBankNameDto dto)
    {
        try
        {
            var entity = await _context.BankNames.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Bank name not found" });

            entity.Name = dto.Name;
            entity.BranchCode = dto.BranchCode;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new BankNameDto
            {
                Id = entity.Id,
                Name = entity.Name,
                BranchCode = entity.BranchCode,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating bank name");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("account-types/{id:guid}")]
    public async Task<IActionResult> UpdateAccountType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.AccountTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Account type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating account type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("units/{id:guid}")]
    public async Task<IActionResult> UpdateUnitOfMeasure(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.UnitsOfMeasure.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Unit of measure not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating unit of measure");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("employment-types/{id:guid}")]
    public async Task<IActionResult> UpdateEmploymentType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.EmploymentTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Employment type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating employment type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("positions/{id:guid}")]
    public async Task<IActionResult> UpdatePosition(Guid id, [FromBody] UpdatePositionDto dto)
    {
        try
        {
            var entity = await _context.Positions.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Position not found" });

            entity.Name = dto.Name;
            entity.Rate = dto.Rate;
            entity.Description = dto.Description;
            entity.IsDriverPosition = dto.IsDriverPosition;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new PositionDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Rate = entity.Rate,
                Description = entity.Description,
                IsDriverPosition = entity.IsDriverPosition,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating position");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("team-types/{id:guid}")]
    public async Task<IActionResult> UpdateTeamType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.TeamTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Team type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating team type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("work-areas/{id:guid}")]
    public async Task<IActionResult> UpdateWorkArea(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.WorkAreas.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Work area not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating work area");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("leave-types/{id:guid}")]
    public async Task<IActionResult> UpdateLeaveType(Guid id, [FromBody] UpdateLeaveTypeDto dto)
    {
        try
        {
            var entity = await _context.LeaveTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Leave type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.RequiresDocument = dto.RequiresDocument;
            entity.DefaultDaysPerYear = dto.DefaultDaysPerYear;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new LeaveTypeDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                RequiresDocument = entity.RequiresDocument,
                DefaultDaysPerYear = entity.DefaultDaysPerYear,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating leave type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("document-types/{id:guid}")]
    public async Task<IActionResult> UpdateDocumentType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.DocumentTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Document type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating document type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("field-phases/{id:guid}")]
    public async Task<IActionResult> UpdateFieldPhase(Guid id, [FromBody] UpdateFieldPhaseDto dto)
    {
        try
        {
            var entity = await _context.FieldPhases.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Field phase not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.SortOrder = dto.SortOrder;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new FieldPhaseDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                SortOrder = entity.SortOrder,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating field phase");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== DELETE ENDPOINTS ==========

    [HttpDelete("bank-names/{id:guid}")]
    public async Task<IActionResult> DeleteBankName(Guid id)
    {
        try
        {
            var entity = await _context.BankNames.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Bank name not found" });

            // Check if used in BankDetails
            var isUsed = await _context.BankDetails.AnyAsync(b => b.BankNameId == id);
            if (isUsed)
            {
                return BadRequest(new { message = "Cannot delete bank name as it is being used in employee bank details" });
            }

            _context.BankNames.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting bank name");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("account-types/{id:guid}")]
    public async Task<IActionResult> DeleteAccountType(Guid id)
    {
        try
        {
            var entity = await _context.AccountTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Account type not found" });

            // Check if used in BankDetails
            var isUsed = await _context.BankDetails.AnyAsync(b => b.AccountTypeId == id);
            if (isUsed)
            {
                return BadRequest(new { message = "Cannot delete account type as it is being used in employee bank details" });
            }

            _context.AccountTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting account type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("units/{id:guid}")]
    public async Task<IActionResult> DeleteUnitOfMeasure(Guid id)
    {
        try
        {
            var entity = await _context.UnitsOfMeasure.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Unit of measure not found" });

            // Check if used in InventoryItems
            var isUsed = await _context.InventoryItems.AnyAsync(i => i.UnitOfMeasureId == id);
            if (isUsed)
            {
                return BadRequest(new { message = "Cannot delete unit of measure as it is being used in inventory items" });
            }

            _context.UnitsOfMeasure.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting unit of measure");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("employment-types/{id:guid}")]
    public async Task<IActionResult> DeleteEmploymentType(Guid id)
    {
        try
        {
            var entity = await _context.EmploymentTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Employment type not found" });

            // Check if used in Employees
            var isUsed = await _context.Employees.AnyAsync(e => e.EmploymentTypeId == id);
            if (isUsed)
            {
                return BadRequest(new { message = "Cannot delete employment type as it is being used by employees" });
            }

            _context.EmploymentTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting employment type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("positions/{id:guid}")]
    public async Task<IActionResult> DeletePosition(Guid id)
    {
        try
        {
            var entity = await _context.Positions.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Position not found" });

            // Check if used in Employees
            var isUsed = await _context.Employees.AnyAsync(e => e.PositionId == id);
            if (isUsed)
            {
                return BadRequest(new { message = "Cannot delete position as it is being used by employees" });
            }

            _context.Positions.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting position");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("team-types/{id:guid}")]
    public async Task<IActionResult> DeleteTeamType(Guid id)
    {
        try
        {
            var entity = await _context.TeamTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Team type not found" });

            // Check if used in Teams
            var isUsed = await _context.Teams.AnyAsync(t => t.TeamTypeId == id);
            if (isUsed)
            {
                return BadRequest(new { message = "Cannot delete team type as it is being used by teams" });
            }

            _context.TeamTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting team type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("work-areas/{id:guid}")]
    public async Task<IActionResult> DeleteWorkArea(Guid id)
    {
        try
        {
            var entity = await _context.WorkAreas.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Work area not found" });

            // Check if used (assuming WorkTasks have WorkAreaId - verify this)
            // var isUsed = await _context.WorkTasks.AnyAsync(w => w.WorkAreaId == id);
            // For now, just allow deletion
            
            _context.WorkAreas.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting work area");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== USAGE CHECK ENDPOINTS ==========

    [HttpGet("bank-names/{id:guid}/usage")]
    public async Task<IActionResult> GetBankNameUsage(Guid id)
    {
        try
        {
            var entity = await _context.BankNames.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Bank name not found" });

            var usageCount = await _context.BankDetails.CountAsync(b => b.BankNameId == id);
            var usedInTables = new List<string>();
            if (usageCount > 0)
                usedInTables.Add("BankDetails");

            var result = new UsageInfoDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UsageCount = usageCount,
                UsedInTables = usedInTables,
                CanDelete = usageCount == 0
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting bank name usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("account-types/{id:guid}/usage")]
    public async Task<IActionResult> GetAccountTypeUsage(Guid id)
    {
        try
        {
            var entity = await _context.AccountTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Account type not found" });

            var usageCount = await _context.BankDetails.CountAsync(b => b.AccountTypeId == id);
            var usedInTables = new List<string>();
            if (usageCount > 0)
                usedInTables.Add("BankDetails");

            var result = new UsageInfoDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UsageCount = usageCount,
                UsedInTables = usedInTables,
                CanDelete = usageCount == 0
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting account type usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("employment-types/{id:guid}/usage")]
    public async Task<IActionResult> GetEmploymentTypeUsage(Guid id)
    {
        try
        {
            var entity = await _context.EmploymentTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Employment type not found" });

            var usageCount = await _context.Employees.CountAsync(e => e.EmploymentTypeId == id);
            var usedInTables = new List<string>();
            if (usageCount > 0)
                usedInTables.Add("Employees");

            var result = new UsageInfoDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UsageCount = usageCount,
                UsedInTables = usedInTables,
                CanDelete = usageCount == 0
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting employment type usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("positions/{id:guid}/usage")]
    public async Task<IActionResult> GetPositionUsage(Guid id)
    {
        try
        {
            var entity = await _context.Positions.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Position not found" });

            var usageCount = await _context.Employees.CountAsync(e => e.PositionId == id);
            var usedInTables = new List<string>();
            if (usageCount > 0)
                usedInTables.Add("Employees");

            var result = new UsageInfoDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UsageCount = usageCount,
                UsedInTables = usedInTables,
                CanDelete = usageCount == 0
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting position usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("team-types/{id:guid}/usage")]
    public async Task<IActionResult> GetTeamTypeUsage(Guid id)
    {
        try
        {
            var entity = await _context.TeamTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Team type not found" });

            var usageCount = await _context.Teams.CountAsync(t => t.TeamTypeId == id);

            return Ok(new UsageInfoDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "Teams" } : new List<string>(),
                CanDelete = usageCount == 0
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting team type usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("work-areas/{id:guid}/usage")]
    public async Task<IActionResult> GetWorkAreaUsage(Guid id)
    {
        try
        {
            var entity = await _context.WorkAreas.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Work area not found" });

            var usageCount = await _context.WorkTasks.CountAsync(w => w.WorkAreaId == id);

            return Ok(new UsageInfoDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "WorkTasks" } : new List<string>(),
                CanDelete = usageCount == 0
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting work area usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("units/{id:guid}/usage")]
    public async Task<IActionResult> GetUnitOfMeasureUsage(Guid id)
    {
        try
        {
            var entity = await _context.UnitsOfMeasure.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Unit of measure not found" });

            var usageCount = await _context.InventoryItems.CountAsync(i => i.UnitOfMeasureId == id);

            return Ok(new UsageInfoDto
            {
                Id = entity.Id,
                Name = entity.Name,
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "InventoryItems" } : new List<string>(),
                CanDelete = usageCount == 0
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting unit of measure usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== REPLACE AND DELETE ENDPOINTS ==========

    [HttpPost("bank-names/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteBankName([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.BankNames.FindAsync(dto.SourceId);
            var target = await _context.BankNames.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Bank name not found" });

            // Replace all references
            var bankDetails = await _context.BankDetails
                .Where(b => b.BankNameId == dto.SourceId)
                .ToListAsync();

            foreach (var detail in bankDetails)
            {
                detail.BankNameId = dto.TargetId;
                detail.UpdatedAt = DateTime.UtcNow;
            }

            // Delete the source
            _context.BankNames.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {bankDetails.Count} references and deleted bank name" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting bank name");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("account-types/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteAccountType([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.AccountTypes.FindAsync(dto.SourceId);
            var target = await _context.AccountTypes.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Account type not found" });

            // Replace all references
            var bankDetails = await _context.BankDetails
                .Where(b => b.AccountTypeId == dto.SourceId)
                .ToListAsync();

            foreach (var detail in bankDetails)
            {
                detail.AccountTypeId = dto.TargetId;
                detail.UpdatedAt = DateTime.UtcNow;
            }

            // Delete the source
            _context.AccountTypes.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {bankDetails.Count} references and deleted account type" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting account type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("employment-types/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteEmploymentType([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.EmploymentTypes.FindAsync(dto.SourceId);
            var target = await _context.EmploymentTypes.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Employment type not found" });

            // Replace all references
            var employees = await _context.Employees
                .Where(e => e.EmploymentTypeId == dto.SourceId)
                .ToListAsync();

            foreach (var employee in employees)
            {
                employee.EmploymentTypeId = dto.TargetId;
                employee.UpdatedAt = DateTime.UtcNow;
            }

            // Delete the source
            _context.EmploymentTypes.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {employees.Count} references and deleted employment type" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting employment type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("positions/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeletePosition([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.Positions.FindAsync(dto.SourceId);
            var target = await _context.Positions.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Position not found" });

            // Replace all references
            var employees = await _context.Employees
                .Where(e => e.PositionId == dto.SourceId)
                .ToListAsync();

            foreach (var employee in employees)
            {
                employee.PositionId = dto.TargetId;
                employee.UpdatedAt = DateTime.UtcNow;
            }

            // Delete the source
            _context.Positions.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {employees.Count} references and deleted position" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting position");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("team-types/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteTeamType([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.TeamTypes.FindAsync(dto.SourceId);
            var target = await _context.TeamTypes.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Team type not found" });

            // Replace all references
            var teams = await _context.Teams
                .Where(t => t.TeamTypeId == dto.SourceId)
                .ToListAsync();

            foreach (var team in teams)
            {
                team.TeamTypeId = dto.TargetId;
                team.UpdatedAt = DateTime.UtcNow;
            }

            // Delete the source
            _context.TeamTypes.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {teams.Count} references and deleted team type" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting team type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("units/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteUnitOfMeasure([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.UnitsOfMeasure.FindAsync(dto.SourceId);
            var target = await _context.UnitsOfMeasure.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Unit of measure not found" });

            // Replace all references in InventoryItems
            var items = await _context.InventoryItems
                .Where(i => i.UnitOfMeasureId == dto.SourceId)
                .ToListAsync();

            foreach (var item in items)
            {
                item.UnitOfMeasureId = dto.TargetId;
                item.UpdatedAt = DateTime.UtcNow;
            }

            // Delete the source
            _context.UnitsOfMeasure.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {items.Count} references and deleted unit of measure" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting unit of measure");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== INVENTORY CATEGORIES CRUD ==========

    [HttpPost("categories")]
    public async Task<IActionResult> CreateCategory([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new InventoryCategory
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.InventoryCategories.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetCategories), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating category");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("categories/{id:guid}")]
    public async Task<IActionResult> UpdateCategory(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.InventoryCategories.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Category not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating category");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("categories/{id:guid}")]
    public async Task<IActionResult> DeleteCategory(Guid id)
    {
        try
        {
            var entity = await _context.InventoryCategories.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Category not found" });

            var usageCount = await _context.InventoryItems.CountAsync(i => i.CategoryId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This category is used by {usageCount} inventory items." });

            _context.InventoryCategories.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting category");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("categories/{id:guid}/usage")]
    public async Task<IActionResult> GetCategoryUsage(Guid id)
    {
        try
        {
            var entity = await _context.InventoryCategories.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Category not found" });

            var usageCount = await _context.InventoryItems.CountAsync(i => i.CategoryId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "InventoryItems" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting category usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("categories/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteCategory([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.InventoryCategories.FindAsync(dto.SourceId);
            var target = await _context.InventoryCategories.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Category not found" });

            var items = await _context.InventoryItems
                .Where(i => i.CategoryId == dto.SourceId)
                .ToListAsync();

            foreach (var item in items)
            {
                item.CategoryId = dto.TargetId;
                item.UpdatedAt = DateTime.UtcNow;
            }

            _context.InventoryCategories.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {items.Count} references and deleted category" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting category");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== INVENTORY TYPES CRUD ==========

    [HttpPost("types")]
    public async Task<IActionResult> CreateType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new InventoryType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.InventoryTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("types/{id:guid}")]
    public async Task<IActionResult> UpdateType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.InventoryTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("types/{id:guid}")]
    public async Task<IActionResult> DeleteType(Guid id)
    {
        try
        {
            var entity = await _context.InventoryTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Type not found" });

            var usageCount = await _context.InventoryItems.CountAsync(i => i.TypeId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This type is used by {usageCount} inventory items." });

            _context.InventoryTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("types/{id:guid}/usage")]
    public async Task<IActionResult> GetTypeUsage(Guid id)
    {
        try
        {
            var entity = await _context.InventoryTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Type not found" });

            var usageCount = await _context.InventoryItems.CountAsync(i => i.TypeId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "InventoryItems" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting type usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("types/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteType([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.InventoryTypes.FindAsync(dto.SourceId);
            var target = await _context.InventoryTypes.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Type not found" });

            var items = await _context.InventoryItems
                .Where(i => i.TypeId == dto.SourceId)
                .ToListAsync();

            foreach (var item in items)
            {
                item.TypeId = dto.TargetId;
                item.UpdatedAt = DateTime.UtcNow;
            }

            _context.InventoryTypes.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {items.Count} references and deleted type" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== EQUIPMENT CONDITIONS CRUD ==========

    [HttpPost("conditions")]
    public async Task<IActionResult> CreateCondition([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new EquipmentCondition
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.EquipmentConditions.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetEquipmentConditions), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating condition");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("conditions/{id:guid}")]
    public async Task<IActionResult> UpdateCondition(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.EquipmentConditions.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Condition not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating condition");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("conditions/{id:guid}")]
    public async Task<IActionResult> DeleteCondition(Guid id)
    {
        try
        {
            var entity = await _context.EquipmentConditions.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Condition not found" });

            _context.EquipmentConditions.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting condition");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("conditions/{id:guid}/usage")]
    public async Task<IActionResult> GetConditionUsage(Guid id)
    {
        try
        {
            var entity = await _context.EquipmentConditions.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Condition not found" });

            return Ok(new UsageInfoDto
            {
                UsageCount = 0,
                UsedInTables = new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting condition usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("conditions/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteCondition([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.EquipmentConditions.FindAsync(dto.SourceId);
            if (source == null)
                return NotFound(new { message = "Condition not found" });

            _context.EquipmentConditions.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Deleted condition" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting condition");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== MAINTENANCE TYPES CRUD ==========

    [HttpPost("maintenance-types")]
    public async Task<IActionResult> CreateMaintenanceType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new FarmSync.Domain.Entities.ReferenceData.MaintenanceType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.MaintenanceTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetMaintenanceTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating maintenance type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("maintenance-types/{id:guid}")]
    public async Task<IActionResult> UpdateMaintenanceType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.MaintenanceTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Maintenance type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating maintenance type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("maintenance-types/{id:guid}")]
    public async Task<IActionResult> DeleteMaintenanceType(Guid id)
    {
        try
        {
            var entity = await _context.MaintenanceTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Maintenance type not found" });

            _context.MaintenanceTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting maintenance type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("maintenance-types/{id:guid}/usage")]
    public async Task<IActionResult> GetMaintenanceTypeUsage(Guid id)
    {
        try
        {
            var entity = await _context.MaintenanceTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Maintenance type not found" });

            return Ok(new UsageInfoDto
            {
                UsageCount = 0,
                UsedInTables = new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting maintenance type usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("maintenance-types/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteMaintenanceType([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.MaintenanceTypes.FindAsync(dto.SourceId);
            if (source == null)
                return NotFound(new { message = "Maintenance type not found" });

            _context.MaintenanceTypes.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Deleted maintenance type" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting maintenance type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== LOCATIONS CRUD ==========

    [HttpPost("locations")]
    public async Task<IActionResult> CreateLocation([FromBody] CreateInventoryLocationDto dto)
    {
        try
        {
            var entity = new InventoryLocation
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                Address = dto.Address,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.InventoryLocations.Add(entity);
            await _context.SaveChangesAsync();

            var result = new InventoryLocationDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Address = entity.Address,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetLocations), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating location");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("locations/{id:guid}")]
    public async Task<IActionResult> UpdateLocation(Guid id, [FromBody] UpdateInventoryLocationDto dto)
    {
        try
        {
            var entity = await _context.InventoryLocations.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Location not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.Address = dto.Address;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new InventoryLocationDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                Address = entity.Address,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating location");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("locations/{id:guid}")]
    public async Task<IActionResult> DeleteLocation(Guid id)
    {
        try
        {
            var entity = await _context.InventoryLocations.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Location not found" });

            var usageCount = await _context.StockLevels.CountAsync(s => s.LocationId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This location is used by {usageCount} stock levels." });

            _context.InventoryLocations.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting location");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("locations/{id:guid}/usage")]
    public async Task<IActionResult> GetLocationUsage(Guid id)
    {
        try
        {
            var entity = await _context.InventoryLocations.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Location not found" });

            var usageCount = await _context.StockLevels.CountAsync(s => s.LocationId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "StockLevels" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting location usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("locations/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteLocation([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.InventoryLocations.FindAsync(dto.SourceId);
            var target = await _context.InventoryLocations.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Location not found" });

            var stockLevels = await _context.StockLevels
                .Where(s => s.LocationId == dto.SourceId)
                .ToListAsync();

            foreach (var level in stockLevels)
            {
                level.LocationId = dto.TargetId;
                level.UpdatedAt = DateTime.UtcNow;
            }

            _context.InventoryLocations.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {stockLevels.Count} references and deleted location" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting location");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== TASK STATUSES CRUD ==========

    [HttpPost("task-statuses")]
    public async Task<IActionResult> CreateTaskStatus([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new FarmSync.Domain.Entities.HR.TaskStatus
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                ColorCode = "#000000",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.TaskStatuses.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetTaskStatuses), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("task-statuses/{id:guid}")]
    public async Task<IActionResult> UpdateTaskStatus(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.TaskStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Task status not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("task-statuses/{id:guid}")]
    public async Task<IActionResult> DeleteTaskStatus(Guid id)
    {
        try
        {
            var entity = await _context.TaskStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Task status not found" });

            var usageCount = await _context.WorkTasks.CountAsync(t => t.TaskStatusId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This status is used by {usageCount} tasks." });

            _context.TaskStatuses.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("task-statuses/{id:guid}/usage")]
    public async Task<IActionResult> GetTaskStatusUsage(Guid id)
    {
        try
        {
            var entity = await _context.TaskStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Task status not found" });

            var usageCount = await _context.WorkTasks.CountAsync(t => t.TaskStatusId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "Tasks" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting task status usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("task-statuses/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteTaskStatus([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.TaskStatuses.FindAsync(dto.SourceId);
            var target = await _context.TaskStatuses.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Task status not found" });

            var tasks = await _context.WorkTasks
                .Where(t => t.TaskStatusId == dto.SourceId)
                .ToListAsync();

            foreach (var task in tasks)
            {
                task.TaskStatusId = dto.TargetId;
                task.UpdatedAt = DateTime.UtcNow;
            }

            _context.TaskStatuses.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {tasks.Count} references and deleted task status" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting task status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== ISSUE STATUSES CRUD ==========

    [HttpPost("issue-statuses")]
    public async Task<IActionResult> CreateIssueStatus([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new IssueStatus
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                ColorCode = "#000000",
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.IssueStatuses.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetIssueStatuses), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating issue status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("issue-statuses/{id:guid}")]
    public async Task<IActionResult> UpdateIssueStatus(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.IssueStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Issue status not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating issue status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("issue-statuses/{id:guid}")]
    public async Task<IActionResult> DeleteIssueStatus(Guid id)
    {
        try
        {
            var entity = await _context.IssueStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Issue status not found" });

            var usageCount = await _context.InventoryIssues.CountAsync(i => i.IssueStatusId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This status is used by {usageCount} issues." });

            _context.IssueStatuses.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting issue status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("issue-statuses/{id:guid}/usage")]
    public async Task<IActionResult> GetIssueStatusUsage(Guid id)
    {
        try
        {
            var entity = await _context.IssueStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Issue status not found" });

            var usageCount = await _context.InventoryIssues.CountAsync(i => i.IssueStatusId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "Issues" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting issue status usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("issue-statuses/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteIssueStatus([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.IssueStatuses.FindAsync(dto.SourceId);
            var target = await _context.IssueStatuses.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Issue status not found" });

            var issues = await _context.InventoryIssues
                .Where(i => i.IssueStatusId == dto.SourceId)
                .ToListAsync();

            foreach (var issue in issues)
            {
                issue.IssueStatusId = dto.TargetId;
                issue.UpdatedAt = DateTime.UtcNow;
            }

            _context.IssueStatuses.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {issues.Count} references and deleted issue status" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting issue status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== TRANSACTION STATUSES CRUD ==========

    [HttpPost("statuses")]
    public async Task<IActionResult> CreateTransactionStatus([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new TransactionStatus
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.TransactionStatuses.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetTransactionStatuses), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating transaction status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("statuses/{id:guid}")]
    public async Task<IActionResult> UpdateTransactionStatus(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.TransactionStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Transaction status not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating transaction status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("statuses/{id:guid}")]
    public async Task<IActionResult> DeleteTransactionStatus(Guid id)
    {
        try
        {
            var entity = await _context.TransactionStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Transaction status not found" });

            var usageCount = await _context.InventoryTransactions.CountAsync(t => t.StatusId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This status is used by {usageCount} transactions." });

            _context.TransactionStatuses.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting transaction status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("statuses/{id:guid}/usage")]
    public async Task<IActionResult> GetTransactionStatusUsage(Guid id)
    {
        try
        {
            var entity = await _context.TransactionStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Transaction status not found" });

            var usageCount = await _context.InventoryTransactions.CountAsync(t => t.StatusId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "InventoryTransactions" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting transaction status usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("statuses/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteTransactionStatus([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.TransactionStatuses.FindAsync(dto.SourceId);
            var target = await _context.TransactionStatuses.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Transaction status not found" });

            var transactions = await _context.InventoryTransactions
                .Where(t => t.StatusId == dto.SourceId)
                .ToListAsync();

            foreach (var transaction in transactions)
            {
                transaction.StatusId = dto.TargetId;
                transaction.UpdatedAt = DateTime.UtcNow;
            }

            _context.TransactionStatuses.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {transactions.Count} references and deleted transaction status" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting transaction status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== VEHICLE TYPES CRUD ==========

    [HttpPost("vehicle-types")]
    public async Task<IActionResult> CreateVehicleType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new VehicleType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.VehicleTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetVehicleTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vehicle type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("vehicle-types/{id:guid}")]
    public async Task<IActionResult> UpdateVehicleType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.VehicleTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Vehicle type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vehicle type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("vehicle-types/{id:guid}")]
    public async Task<IActionResult> DeleteVehicleType(Guid id)
    {
        try
        {
            var entity = await _context.VehicleTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Vehicle type not found" });

            var usageCount = await _context.Vehicles.CountAsync(v => v.VehicleTypeId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This vehicle type is used by {usageCount} vehicles." });

            _context.VehicleTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting vehicle type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("vehicle-types/{id:guid}/usage")]
    public async Task<IActionResult> GetVehicleTypeUsage(Guid id)
    {
        try
        {
            var entity = await _context.VehicleTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Vehicle type not found" });

            var usageCount = await _context.Vehicles.CountAsync(v => v.VehicleTypeId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "Vehicles" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicle type usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("vehicle-types/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteVehicleType([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.VehicleTypes.FindAsync(dto.SourceId);
            var target = await _context.VehicleTypes.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Vehicle type not found" });

            var vehicles = await _context.Vehicles
                .Where(v => v.VehicleTypeId == dto.SourceId)
                .ToListAsync();

            foreach (var vehicle in vehicles)
            {
                vehicle.VehicleTypeId = dto.TargetId;
                vehicle.UpdatedAt = DateTime.UtcNow;
            }

            _context.VehicleTypes.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {vehicles.Count} references and deleted vehicle type" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting vehicle type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== VEHICLE STATUSES CRUD ==========

    [HttpPost("vehicle-statuses")]
    public async Task<IActionResult> CreateVehicleStatus([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new VehicleStatus
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.VehicleStatuses.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetVehicleStatuses), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating vehicle status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("vehicle-statuses/{id:guid}")]
    public async Task<IActionResult> UpdateVehicleStatus(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.VehicleStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Vehicle status not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating vehicle status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("vehicle-statuses/{id:guid}")]
    public async Task<IActionResult> DeleteVehicleStatus(Guid id)
    {
        try
        {
            var entity = await _context.VehicleStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Vehicle status not found" });

            var usageCount = await _context.Vehicles.CountAsync(v => v.VehicleStatusId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This status is used by {usageCount} vehicles." });

            _context.VehicleStatuses.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting vehicle status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("vehicle-statuses/{id:guid}/usage")]
    public async Task<IActionResult> GetVehicleStatusUsage(Guid id)
    {
        try
        {
            var entity = await _context.VehicleStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Vehicle status not found" });

            var usageCount = await _context.Vehicles.CountAsync(v => v.VehicleStatusId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "Vehicles" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting vehicle status usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("vehicle-statuses/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteVehicleStatus([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.VehicleStatuses.FindAsync(dto.SourceId);
            var target = await _context.VehicleStatuses.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Vehicle status not found" });

            var vehicles = await _context.Vehicles
                .Where(v => v.VehicleStatusId == dto.SourceId)
                .ToListAsync();

            foreach (var vehicle in vehicles)
            {
                vehicle.VehicleStatusId = dto.TargetId;
                vehicle.UpdatedAt = DateTime.UtcNow;
            }

            _context.VehicleStatuses.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {vehicles.Count} references and deleted vehicle status" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting vehicle status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== FUEL TYPES CRUD ==========

    [HttpPost("fuel-types")]
    public async Task<IActionResult> CreateFuelType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new FuelType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.FuelTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetFuelTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating fuel type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("fuel-types/{id:guid}")]
    public async Task<IActionResult> UpdateFuelType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.FuelTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fuel type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating fuel type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("fuel-types/{id:guid}")]
    public async Task<IActionResult> DeleteFuelType(Guid id)
    {
        try
        {
            var entity = await _context.FuelTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fuel type not found" });

            _context.FuelTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting fuel type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("fuel-types/{id:guid}/usage")]
    public async Task<IActionResult> GetFuelTypeUsage(Guid id)
    {
        try
        {
            var entity = await _context.FuelTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fuel type not found" });

            return Ok(new UsageInfoDto
            {
                UsageCount = 0,
                UsedInTables = new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fuel type usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("fuel-types/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteFuelType([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.FuelTypes.FindAsync(dto.SourceId);
            if (source == null)
                return NotFound(new { message = "Fuel type not found" });

            _context.FuelTypes.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = "Deleted fuel type" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting fuel type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== FLEET MAINTENANCE TYPES CRUD ==========

    [HttpPost("fleet-maintenance-types")]
    public async Task<IActionResult> CreateFleetMaintenanceType([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new FarmSync.Domain.Entities.Fleet.MaintenanceType
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.FleetMaintenanceTypes.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetFleetMaintenanceTypes), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating fleet maintenance type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("fleet-maintenance-types/{id:guid}")]
    public async Task<IActionResult> UpdateFleetMaintenanceType(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.FleetMaintenanceTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fleet maintenance type not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating fleet maintenance type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("fleet-maintenance-types/{id:guid}")]
    public async Task<IActionResult> DeleteFleetMaintenanceType(Guid id)
    {
        try
        {
            var entity = await _context.FleetMaintenanceTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fleet maintenance type not found" });

            var usageCount = await _context.MaintenanceRecords.CountAsync(m => m.MaintenanceTypeId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This type is used by {usageCount} maintenance records." });

            _context.FleetMaintenanceTypes.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting fleet maintenance type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("fleet-maintenance-types/{id:guid}/usage")]
    public async Task<IActionResult> GetFleetMaintenanceTypeUsage(Guid id)
    {
        try
        {
            var entity = await _context.FleetMaintenanceTypes.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fleet maintenance type not found" });

            var usageCount = await _context.MaintenanceRecords.CountAsync(m => m.MaintenanceTypeId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "FleetMaintenanceRecords" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fleet maintenance type usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("fleet-maintenance-types/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteFleetMaintenanceType([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.FleetMaintenanceTypes.FindAsync(dto.SourceId);
            var target = await _context.FleetMaintenanceTypes.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Fleet maintenance type not found" });

            var records = await _context.MaintenanceRecords
                .Where(m => m.MaintenanceTypeId == dto.SourceId)
                .ToListAsync();

            foreach (var record in records)
            {
                record.MaintenanceTypeId = dto.TargetId;
                record.UpdatedAt = DateTime.UtcNow;
            }

            _context.FleetMaintenanceTypes.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {records.Count} references and deleted fleet maintenance type" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting fleet maintenance type");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    // ========== FLEET TASK STATUSES CRUD ==========

    [HttpPost("fleet-task-statuses")]
    public async Task<IActionResult> CreateFleetTaskStatus([FromBody] CreateReferenceDto dto)
    {
        try
        {
            var entity = new FarmSync.Domain.Entities.Fleet.TaskStatus
            {
                Id = Guid.NewGuid(),
                Name = dto.Name,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow
            };

            _context.FleetTaskStatuses.Add(entity);
            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return CreatedAtAction(nameof(GetFleetTaskStatuses), new { id = entity.Id }, result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating fleet task status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPut("fleet-task-statuses/{id:guid}")]
    public async Task<IActionResult> UpdateFleetTaskStatus(Guid id, [FromBody] UpdateReferenceDto dto)
    {
        try
        {
            var entity = await _context.FleetTaskStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fleet task status not found" });

            entity.Name = dto.Name;
            entity.Description = dto.Description;
            entity.IsActive = dto.IsActive;
            entity.UpdatedAt = DateTime.UtcNow;

            await _context.SaveChangesAsync();

            var result = new ReferenceDto
            {
                Id = entity.Id,
                Name = entity.Name,
                Description = entity.Description,
                IsActive = entity.IsActive
            };

            return Ok(result);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating fleet task status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpDelete("fleet-task-statuses/{id:guid}")]
    public async Task<IActionResult> DeleteFleetTaskStatus(Guid id)
    {
        try
        {
            var entity = await _context.FleetTaskStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fleet task status not found" });

            var usageCount = await _context.TransportTasks.CountAsync(t => t.TaskStatusId == id);
            if (usageCount > 0)
                return BadRequest(new { message = $"Cannot delete. This status is used by {usageCount} fleet tasks." });

            _context.FleetTaskStatuses.Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting fleet task status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpGet("fleet-task-statuses/{id:guid}/usage")]
    public async Task<IActionResult> GetFleetTaskStatusUsage(Guid id)
    {
        try
        {
            var entity = await _context.FleetTaskStatuses.FindAsync(id);
            if (entity == null)
                return NotFound(new { message = "Fleet task status not found" });

            var usageCount = await _context.TransportTasks.CountAsync(t => t.TaskStatusId == id);

            return Ok(new UsageInfoDto
            {
                UsageCount = usageCount,
                UsedInTables = usageCount > 0 ? new List<string> { "FleetTasks" } : new List<string>()
            });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting fleet task status usage");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }

    [HttpPost("fleet-task-statuses/replace-and-delete")]
    public async Task<IActionResult> ReplaceAndDeleteFleetTaskStatus([FromBody] ReplaceAndDeleteDto dto)
    {
        try
        {
            var source = await _context.FleetTaskStatuses.FindAsync(dto.SourceId);
            var target = await _context.FleetTaskStatuses.FindAsync(dto.TargetId);

            if (source == null || target == null)
                return NotFound(new { message = "Fleet task status not found" });

            var tasks = await _context.TransportTasks
                .Where(t => t.TaskStatusId == dto.SourceId)
                .ToListAsync();

            foreach (var task in tasks)
            {
                task.TaskStatusId = dto.TargetId;
                task.UpdatedAt = DateTime.UtcNow;
            }

            _context.FleetTaskStatuses.Remove(source);
            await _context.SaveChangesAsync();

            return Ok(new { success = true, message = $"Replaced {tasks.Count} references and deleted fleet task status" });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error replacing and deleting fleet task status");
            return StatusCode(500, new { message = "An error occurred" });
        }
    }
}


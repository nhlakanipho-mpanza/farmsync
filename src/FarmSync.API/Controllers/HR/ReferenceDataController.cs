using FarmSync.Application.DTOs.HR;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers.HR;

[Authorize]
[ApiController]
[Route("api/hr/[controller]")]
public class ReferenceDataController : ControllerBase
{
    private readonly IRepository<Position> _positionRepository;
    private readonly IRepository<EmploymentType> _employmentTypeRepository;
    private readonly IRepository<RoleType> _roleTypeRepository;
    private readonly IRepository<TeamType> _teamTypeRepository;
    private readonly IRepository<BankName> _bankNameRepository;
    private readonly IRepository<AccountType> _accountTypeRepository;
    private readonly IRepository<Domain.Entities.HR.TaskStatus> _taskStatusRepository;
    private readonly IRepository<IssueStatus> _issueStatusRepository;
    private readonly IRepository<WorkArea> _workAreaRepository;

    public ReferenceDataController(
        IRepository<Position> positionRepository,
        IRepository<EmploymentType> employmentTypeRepository,
        IRepository<RoleType> roleTypeRepository,
        IRepository<TeamType> teamTypeRepository,
        IRepository<BankName> bankNameRepository,
        IRepository<AccountType> accountTypeRepository,
        IRepository<Domain.Entities.HR.TaskStatus> taskStatusRepository,
        IRepository<IssueStatus> issueStatusRepository,
        IRepository<WorkArea> workAreaRepository)
    {
        _positionRepository = positionRepository;
        _employmentTypeRepository = employmentTypeRepository;
        _roleTypeRepository = roleTypeRepository;
        _teamTypeRepository = teamTypeRepository;
        _bankNameRepository = bankNameRepository;
        _accountTypeRepository = accountTypeRepository;
        _taskStatusRepository = taskStatusRepository;
        _issueStatusRepository = issueStatusRepository;
        _workAreaRepository = workAreaRepository;
    }

    [HttpGet("positions")]
    public async Task<ActionResult<IEnumerable<PositionDTO>>> GetPositions()
    {
        var positions = await _positionRepository.GetAllAsync();
        return Ok(positions.Select(p => new PositionDTO
        {
            Id = p.Id,
            Name = p.Name,
            Rate = p.Rate,
            Description = p.Description,
            IsActive = p.IsActive,
            CreatedAt = p.CreatedAt
        }));
    }

    [HttpGet("employment-types")]
    public async Task<ActionResult<IEnumerable<EmploymentTypeDTO>>> GetEmploymentTypes()
    {
        var types = await _employmentTypeRepository.GetAllAsync();
        return Ok(types.Where(t => t.IsActive).Select(t => new EmploymentTypeDTO
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            IsActive = t.IsActive
        }));
    }

    [HttpGet("role-types")]
    public async Task<ActionResult<IEnumerable<RoleTypeDTO>>> GetRoleTypes()
    {
        var types = await _roleTypeRepository.GetAllAsync();
        return Ok(types.Where(t => t.IsActive).Select(t => new RoleTypeDTO
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            IsActive = t.IsActive
        }));
    }

    [HttpGet("team-types")]
    public async Task<ActionResult<IEnumerable<TeamTypeDTO>>> GetTeamTypes()
    {
        var types = await _teamTypeRepository.GetAllAsync();
        return Ok(types.Where(t => t.IsActive).Select(t => new TeamTypeDTO
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            IsActive = t.IsActive
        }));
    }

    [HttpGet("banks")]
    public async Task<ActionResult<IEnumerable<BankNameDTO>>> GetBanks()
    {
        var banks = await _bankNameRepository.GetAllAsync();
        return Ok(banks.Where(b => b.IsActive).Select(b => new BankNameDTO
        {
            Id = b.Id,
            Name = b.Name,
            Description = b.BranchCode,
            BranchCode = b.BranchCode,
            IsActive = b.IsActive
        }));
    }

    [HttpGet("account-types")]
    public async Task<ActionResult<IEnumerable<AccountTypeDTO>>> GetAccountTypes()
    {
        var types = await _accountTypeRepository.GetAllAsync();
        return Ok(types.Where(t => t.IsActive).Select(t => new AccountTypeDTO
        {
            Id = t.Id,
            Name = t.Name,
            Description = t.Description,
            IsActive = t.IsActive
        }));
    }

    [HttpGet("task-statuses")]
    public async Task<ActionResult<IEnumerable<TaskStatusDTO>>> GetTaskStatuses()
    {
        var statuses = await _taskStatusRepository.GetAllAsync();
        return Ok(statuses.Where(s => s.IsActive).Select(s => new TaskStatusDTO
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            ColorCode = s.ColorCode,
            IsActive = s.IsActive
        }));
    }

    [HttpGet("issue-statuses")]
    public async Task<ActionResult<IEnumerable<IssueStatusDTO>>> GetIssueStatuses()
    {
        var statuses = await _issueStatusRepository.GetAllAsync();
        return Ok(statuses.Where(s => s.IsActive).Select(s => new IssueStatusDTO
        {
            Id = s.Id,
            Name = s.Name,
            Description = s.Description,
            ColorCode = s.ColorCode,
            IsActive = s.IsActive
        }));
    }

    [HttpGet("work-areas")]
    public async Task<ActionResult<IEnumerable<WorkAreaDTO>>> GetWorkAreas()
    {
        var areas = await _workAreaRepository.GetAllAsync();
        return Ok(areas.Where(a => a.IsActive).Select(a => new WorkAreaDTO
        {
            Id = a.Id,
            Name = a.Name,
            Size = a.Size,
            Unit = a.SizeUnit,
            Location = a.Location,
            Description = a.Description,
            IsActive = a.IsActive,
            CreatedAt = a.CreatedAt,
            UpdatedAt = a.UpdatedAt
        }));
    }
}

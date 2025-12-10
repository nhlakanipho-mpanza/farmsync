using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers.HR;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class WorkTasksController : ControllerBase
{
    private readonly IWorkTaskService _taskService;

    public WorkTasksController(IWorkTaskService taskService)
    {
        _taskService = taskService;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetAll()
    {
        var tasks = await _taskService.GetAllTasksAsync();
        return Ok(tasks);
    }

    [HttpGet("status/{statusId}")]
    public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetByStatus(Guid statusId)
    {
        var tasks = await _taskService.GetTasksByStatusAsync(statusId);
        return Ok(tasks);
    }

    [HttpGet("team/{teamId}")]
    public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetByTeam(Guid teamId)
    {
        var tasks = await _taskService.GetTasksByTeamAsync(teamId);
        return Ok(tasks);
    }

    [HttpGet("employee/{employeeId}")]
    public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetByEmployee(Guid employeeId)
    {
        var tasks = await _taskService.GetTasksByEmployeeAsync(employeeId);
        return Ok(tasks);
    }

    [HttpGet("scheduled/{date}")]
    public async Task<ActionResult<IEnumerable<WorkTaskDTO>>> GetScheduled(DateOnly date)
    {
        var tasks = await _taskService.GetScheduledTasksAsync(date);
        return Ok(tasks);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<WorkTaskDTO>> GetById(Guid id)
    {
        var task = await _taskService.GetTaskByIdAsync(id);
        if (task == null)
        {
            return NotFound($"Task with ID {id} not found.");
        }
        return Ok(task);
    }

    [HttpPost]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<WorkTaskDTO>> Create([FromBody] CreateWorkTaskDTO dto)
    {
        var task = await _taskService.CreateTaskAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
    }

    [HttpPost("from-template")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<WorkTaskDTO>> CreateFromTemplate([FromBody] CreateTaskFromTemplateDTO dto)
    {
        try
        {
            var task = await _taskService.CreateTaskFromTemplateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = task.Id }, task);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<WorkTaskDTO>> Update(Guid id, [FromBody] UpdateWorkTaskDTO dto)
    {
        try
        {
            var task = await _taskService.UpdateTaskAsync(id, dto);
            return Ok(task);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult> Delete(Guid id)
    {
        await _taskService.DeleteTaskAsync(id);
        return NoContent();
    }

    // Checklist progress tracking endpoints
    [HttpGet("{taskId}/checklist")]
    public async Task<ActionResult<List<TaskChecklistProgressDTO>>> GetTaskChecklist(Guid taskId)
    {
        try
        {
            var checklist = await _taskService.GetTaskChecklistAsync(taskId);
            return Ok(checklist);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{taskId}/checklist/{itemId}/complete")]
    [Authorize(Roles = "Admin,Manager,Operations")]
    public async Task<ActionResult<TaskChecklistProgressDTO>> MarkChecklistItemComplete(
        Guid taskId, 
        Guid itemId,
        [FromBody] CompleteChecklistItemDTO dto)
    {
        try
        {
            var progress = await _taskService.MarkChecklistItemCompleteAsync(
                taskId, 
                itemId, 
                dto.CompletedBy, 
                dto.Notes);
            return Ok(progress);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }

    [HttpPut("{taskId}/checklist/{itemId}/incomplete")]
    [Authorize(Roles = "Admin,Manager")]
    public async Task<ActionResult<TaskChecklistProgressDTO>> MarkChecklistItemIncomplete(Guid taskId, Guid itemId)
    {
        try
        {
            var progress = await _taskService.MarkChecklistItemIncompleteAsync(taskId, itemId);
            return Ok(progress);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(ex.Message);
        }
    }
}

public class CompleteChecklistItemDTO
{
    public Guid CompletedBy { get; set; }
    public string? Notes { get; set; }
}

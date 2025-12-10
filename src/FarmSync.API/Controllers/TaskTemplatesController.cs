using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Application.DTOs.HR;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class TaskTemplatesController : ControllerBase
{
    private readonly ITaskTemplateService _templateService;
    private readonly ILogger<TaskTemplatesController> _logger;

    public TaskTemplatesController(ITaskTemplateService templateService, ILogger<TaskTemplatesController> logger)
    {
        _templateService = templateService;
        _logger = logger;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<TaskTemplateDTO>>> GetAll()
    {
        try
        {
            var templates = await _templateService.GetAllTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task templates");
            return StatusCode(500, new { message = "An error occurred while retrieving task templates" });
        }
    }

    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<TaskTemplateDTO>>> GetActive()
    {
        try
        {
            var templates = await _templateService.GetActiveTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving active task templates");
            return StatusCode(500, new { message = "An error occurred while retrieving active task templates" });
        }
    }

    [HttpGet("category/{category}")]
    public async Task<ActionResult<IEnumerable<TaskTemplateDTO>>> GetByCategory(string category)
    {
        try
        {
            var templates = await _templateService.GetByCategoryAsync(category);
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task templates for category {Category}", category);
            return StatusCode(500, new { message = "An error occurred while retrieving task templates" });
        }
    }

    [HttpGet("recurring")]
    public async Task<ActionResult<IEnumerable<TaskTemplateDTO>>> GetRecurring()
    {
        try
        {
            var templates = await _templateService.GetRecurringTemplatesAsync();
            return Ok(templates);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving recurring task templates");
            return StatusCode(500, new { message = "An error occurred while retrieving recurring task templates" });
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult<TaskTemplateDTO>> GetById(Guid id)
    {
        try
        {
            var template = await _templateService.GetTemplateByIdAsync(id);
            if (template == null)
                return NotFound(new { message = $"Task template with ID {id} not found" });

            return Ok(template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error retrieving task template {Id}", id);
            return StatusCode(500, new { message = "An error occurred while retrieving the task template" });
        }
    }

    [HttpPost]
    public async Task<ActionResult<TaskTemplateDTO>> Create([FromBody] CreateTaskTemplateDTO dto)
    {
        try
        {
            var template = await _templateService.CreateTemplateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = template.Id }, template);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating task template");
            return StatusCode(500, new { message = "An error occurred while creating the task template" });
        }
    }

    [HttpPut("{id}")]
    public async Task<ActionResult<TaskTemplateDTO>> Update(Guid id, [FromBody] UpdateTaskTemplateDTO dto)
    {
        try
        {
            var template = await _templateService.UpdateTemplateAsync(id, dto);
            return Ok(template);
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Task template {Id} not found for update", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating task template {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the task template" });
        }
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> Delete(Guid id)
    {
        try
        {
            await _templateService.DeleteTemplateAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            _logger.LogWarning(ex, "Task template {Id} not found for deletion", id);
            return NotFound(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting task template {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the task template" });
        }
    }
}

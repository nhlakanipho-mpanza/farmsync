using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.HR;

namespace FarmSync.Application.Services.HR;

public class TaskTemplateService : ITaskTemplateService
{
    private readonly ITaskTemplateRepository _templateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TaskTemplateService(ITaskTemplateRepository templateRepository, IUnitOfWork unitOfWork)
    {
        _templateRepository = templateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TaskTemplateDTO>> GetAllTemplatesAsync()
    {
        var templates = await _templateRepository.GetAllAsync();
        return templates.Select(MapToDto);
    }

    public async Task<IEnumerable<TaskTemplateDTO>> GetActiveTemplatesAsync()
    {
        var templates = await _templateRepository.GetActivTemplatesAsync();
        return templates.Select(MapToDto);
    }

    public async Task<IEnumerable<TaskTemplateDTO>> GetByCategoryAsync(string category)
    {
        var templates = await _templateRepository.GetByCategoryAsync(category);
        return templates.Select(MapToDto);
    }

    public async Task<IEnumerable<TaskTemplateDTO>> GetRecurringTemplatesAsync()
    {
        var templates = await _templateRepository.GetRecurringTemplatesAsync();
        return templates.Select(MapToDto);
    }

    public async Task<TaskTemplateDTO?> GetTemplateByIdAsync(Guid id)
    {
        var template = await _templateRepository.GetByIdWithChecklistAsync(id);
        return template == null ? null : MapToDto(template);
    }

    public async Task<TaskTemplateDTO> CreateTemplateAsync(CreateTaskTemplateDTO dto)
    {
        var template = new TaskTemplate
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            Category = dto.Category,
            EstimatedHours = dto.EstimatedHours,
            IsRecurring = dto.IsRecurring,
            RecurrencePattern = dto.RecurrencePattern,
            RecurrenceInterval = dto.RecurrenceInterval,
            Instructions = dto.Instructions,
            IsActive = true,
            ChecklistItems = dto.ChecklistItems.Select(c => new TaskChecklistItem
            {
                Id = Guid.NewGuid(),
                Sequence = c.Sequence,
                Description = c.Description,
                IsRequired = c.IsRequired,
                Notes = c.Notes
            }).ToList(),
            InventoryItems = dto.InventoryItems.Select(i => new TaskTemplateInventoryItem
            {
                Id = Guid.NewGuid(),
                InventoryItemId = i.InventoryItemId,
                QuantityPerUnit = i.QuantityPerUnit,
                AllocationMethod = i.AllocationMethod,
                Notes = i.Notes,
                Sequence = i.Sequence
            }).ToList()
        };

        var created = await _templateRepository.CreateAsync(template);

        return await GetTemplateByIdAsync(created.Id) ?? throw new Exception("Failed to retrieve created template");
    }

    public async Task<TaskTemplateDTO> UpdateTemplateAsync(Guid id, UpdateTaskTemplateDTO dto)
    {
        var template = await _templateRepository.GetByIdWithChecklistAsync(id);
        if (template == null)
            throw new KeyNotFoundException($"Task template with ID {id} not found");

        template.Name = dto.Name;
        template.Description = dto.Description;
        template.Category = dto.Category;
        template.EstimatedHours = dto.EstimatedHours;
        template.IsRecurring = dto.IsRecurring;
        template.RecurrencePattern = dto.RecurrencePattern;
        template.RecurrenceInterval = dto.RecurrenceInterval;
        template.Instructions = dto.Instructions;
        template.IsActive = dto.IsActive;

        // Update checklist items - remove old ones and add new ones
        template.ChecklistItems.Clear();
        template.ChecklistItems = dto.ChecklistItems.Select(c => new TaskChecklistItem
        {
            Id = Guid.NewGuid(),
            TaskTemplateId = template.Id,
            Sequence = c.Sequence,
            Description = c.Description,
            IsRequired = c.IsRequired,
            Notes = c.Notes
        }).ToList();

        // Update inventory items - remove old ones and add new ones
        template.InventoryItems.Clear();
        template.InventoryItems = dto.InventoryItems.Select(i => new TaskTemplateInventoryItem
        {
            Id = Guid.NewGuid(),
            TaskTemplateId = template.Id,
            InventoryItemId = i.InventoryItemId,
            QuantityPerUnit = i.QuantityPerUnit,
            AllocationMethod = i.AllocationMethod,
            Notes = i.Notes,
            Sequence = i.Sequence
        }).ToList();

        await _templateRepository.UpdateAsync(template);

        return await GetTemplateByIdAsync(id) ?? throw new Exception("Failed to retrieve updated template");
    }

    public async Task DeleteTemplateAsync(Guid id)
    {
        await _templateRepository.DeleteAsync(id);
    }

    private static TaskTemplateDTO MapToDto(TaskTemplate template)
    {
        return new TaskTemplateDTO
        {
            Id = template.Id,
            Name = template.Name,
            Description = template.Description,
            Category = template.Category,
            EstimatedHours = template.EstimatedHours,
            IsRecurring = template.IsRecurring,
            RecurrencePattern = template.RecurrencePattern,
            RecurrenceInterval = template.RecurrenceInterval,
            Instructions = template.Instructions,
            IsActive = template.IsActive,
            ChecklistItems = template.ChecklistItems.Select(c => new TaskChecklistItemDTO
            {
                Id = c.Id,
                TaskTemplateId = c.TaskTemplateId,
                Sequence = c.Sequence,
                Description = c.Description,
                IsRequired = c.IsRequired,
                Notes = c.Notes
            }).OrderBy(c => c.Sequence).ToList(),
            InventoryItems = template.InventoryItems.Select(i => new TaskTemplateInventoryItemDTO
            {
                Id = i.Id,
                TaskTemplateId = i.TaskTemplateId,
                InventoryItemId = i.InventoryItemId,
                InventoryItemName = i.InventoryItem?.Name ?? string.Empty,
                InventoryItemCode = i.InventoryItem?.SKU,
                UnitOfMeasure = i.InventoryItem?.UnitOfMeasure?.Name,
                QuantityPerUnit = i.QuantityPerUnit,
                AllocationMethod = i.AllocationMethod,
                Notes = i.Notes,
                Sequence = i.Sequence
            }).OrderBy(i => i.Sequence).ToList(),
            CreatedAt = template.CreatedAt,
            UpdatedAt = template.UpdatedAt
        };
    }
}

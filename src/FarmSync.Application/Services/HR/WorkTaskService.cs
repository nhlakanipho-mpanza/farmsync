using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.HR;

namespace FarmSync.Application.Services.HR;

public class WorkTaskService : IWorkTaskService
{
    private readonly IWorkTaskRepository _taskRepository;
    private readonly ITaskTemplateRepository _templateRepository;
    private readonly IUnitOfWork _unitOfWork;

    public WorkTaskService(
        IWorkTaskRepository taskRepository, 
        ITaskTemplateRepository templateRepository,
        IUnitOfWork unitOfWork)
    {
        _taskRepository = taskRepository;
        _templateRepository = templateRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetAllTasksAsync()
    {
        var tasks = await _taskRepository.GetAllAsync();
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetTasksByStatusAsync(Guid statusId)
    {
        var tasks = await _taskRepository.GetByStatusAsync(statusId);
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetTasksByTeamAsync(Guid teamId)
    {
        var tasks = await _taskRepository.GetByTeamAsync(teamId);
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetTasksByEmployeeAsync(Guid employeeId)
    {
        var tasks = await _taskRepository.GetByEmployeeAsync(employeeId);
        return tasks.Select(MapToDto);
    }

    public async Task<IEnumerable<WorkTaskDTO>> GetScheduledTasksAsync(DateOnly date)
    {
        var tasks = await _taskRepository.GetScheduledTasksAsync(date);
        return tasks.Select(MapToDto);
    }

    public async Task<WorkTaskDTO?> GetTaskByIdAsync(Guid id)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        return task != null ? MapToDto(task) : null;
    }

    public async Task<WorkTaskDTO> CreateTaskAsync(CreateWorkTaskDTO dto)
    {
        var task = new WorkTask
        {
            Id = Guid.NewGuid(),
            TaskDescription = dto.TaskDescription,
            WorkAreaId = dto.WorkAreaId,
            TeamId = dto.TeamId,
            EmployeeId = dto.EmployeeId,
            TaskStatusId = dto.TaskStatusId,
            ScheduledDate = dto.ScheduledDate,
            EstimatedHours = dto.EstimatedHours,
            Notes = dto.Notes
        };

        await _taskRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<WorkTaskDTO> CreateTaskFromTemplateAsync(CreateTaskFromTemplateDTO dto)
    {
        // Get template with checklist items
        var template = await _templateRepository.GetByIdWithChecklistAsync(dto.TaskTemplateId);
        if (template == null)
        {
            throw new KeyNotFoundException($"Task template with ID {dto.TaskTemplateId} not found");
        }

        // Create task from template
        var task = new WorkTask
        {
            Id = Guid.NewGuid(),
            TaskTemplateId = dto.TaskTemplateId,
            TaskDescription = template.Name,
            WorkAreaId = dto.WorkAreaId,
            TeamId = dto.TeamId,
            EmployeeId = dto.EmployeeId,
            TaskStatusId = dto.TaskStatusId,
            ScheduledDate = dto.ScheduledDate,
            EstimatedHours = template.EstimatedHours,
            Notes = string.IsNullOrWhiteSpace(dto.AdditionalNotes) 
                ? template.Instructions 
                : $"{template.Instructions}\n\n{dto.AdditionalNotes}"
        };

        // Create checklist progress entries for each checklist item
        foreach (var checklistItem in template.ChecklistItems)
        {
            task.ChecklistProgress.Add(new TaskChecklistProgress
            {
                Id = Guid.NewGuid(),
                WorkTaskId = task.Id,
                TaskChecklistItemId = checklistItem.Id,
                IsCompleted = false
            });
        }

        // Create inventory allocations from template inventory items
        foreach (var templateInventoryItem in template.InventoryItems)
        {
            // Check if user provided custom quantity in dto
            var userAllocation = dto.InventoryAllocations
                .FirstOrDefault(a => a.TaskTemplateInventoryItemId == templateInventoryItem.Id);

            var plannedQuantity = userAllocation?.PlannedQuantity ?? templateInventoryItem.QuantityPerUnit;
            var teamMemberCount = userAllocation?.TeamMemberCount;

            task.InventoryAllocations.Add(new TaskInventoryAllocation
            {
                Id = Guid.NewGuid(),
                WorkTaskId = task.Id,
                InventoryItemId = templateInventoryItem.InventoryItemId,
                TaskTemplateInventoryItemId = templateInventoryItem.Id,
                PlannedQuantity = plannedQuantity,
                AllocationMethod = templateInventoryItem.AllocationMethod,
                TeamMemberCount = teamMemberCount,
                Notes = userAllocation?.Notes ?? templateInventoryItem.Notes,
                IsIssued = false
            });
        }

        // Also add any additional inventory allocations not from template
        foreach (var allocation in dto.InventoryAllocations.Where(a => !a.TaskTemplateInventoryItemId.HasValue))
        {
            task.InventoryAllocations.Add(new TaskInventoryAllocation
            {
                Id = Guid.NewGuid(),
                WorkTaskId = task.Id,
                InventoryItemId = allocation.InventoryItemId,
                PlannedQuantity = allocation.PlannedQuantity,
                AllocationMethod = allocation.AllocationMethod,
                TeamMemberCount = allocation.TeamMemberCount,
                Notes = allocation.Notes,
                IsIssued = false
            });
        }

        await _taskRepository.AddAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task<WorkTaskDTO> UpdateTaskAsync(Guid id, UpdateWorkTaskDTO dto)
    {
        var task = await _taskRepository.GetByIdAsync(id);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {id} not found");
        }

        task.TaskDescription = dto.TaskDescription;
        task.WorkAreaId = dto.WorkAreaId;
        task.TeamId = dto.TeamId;
        task.EmployeeId = dto.EmployeeId;
        task.TaskStatusId = dto.TaskStatusId;
        task.ScheduledDate = dto.ScheduledDate;
        task.StartDate = dto.StartDate;
        task.CompletedDate = dto.CompletedDate;
        task.EstimatedHours = dto.EstimatedHours;
        task.ActualHours = dto.ActualHours;
        task.Notes = dto.Notes;

        await _taskRepository.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(task);
    }

    public async Task DeleteTaskAsync(Guid id)
    {
        await _taskRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<List<TaskChecklistProgressDTO>> GetTaskChecklistAsync(Guid taskId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found");
        }

        return task.ChecklistProgress
            .OrderBy(cp => cp.TaskChecklistItem?.Sequence)
            .Select(cp => new TaskChecklistProgressDTO
            {
                Id = cp.Id,
                WorkTaskId = cp.WorkTaskId,
                TaskChecklistItemId = cp.TaskChecklistItemId,
                ChecklistItemDescription = cp.TaskChecklistItem?.Description ?? "",
                Sequence = cp.TaskChecklistItem?.Sequence ?? 0,
                IsRequired = cp.TaskChecklistItem?.IsRequired ?? false,
                IsCompleted = cp.IsCompleted,
                CompletedAt = cp.CompletedAt,
                CompletedBy = cp.CompletedBy,
                CompletedByName = cp.CompletedByEmployee?.FullName,
                CompletionNotes = cp.CompletionNotes
            })
            .ToList();
    }

    public async Task<TaskChecklistProgressDTO> MarkChecklistItemCompleteAsync(Guid taskId, Guid itemId, Guid completedBy, string? notes)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found");
        }

        var progress = task.ChecklistProgress.FirstOrDefault(cp => cp.TaskChecklistItemId == itemId);
        if (progress == null)
        {
            throw new KeyNotFoundException($"Checklist item {itemId} not found in task {taskId}");
        }

        progress.IsCompleted = true;
        progress.CompletedAt = DateTime.UtcNow;
        progress.CompletedBy = completedBy;
        progress.CompletionNotes = notes;
        progress.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        // Reload to get updated navigation properties
        task = await _taskRepository.GetByIdAsync(taskId);
        progress = task!.ChecklistProgress.First(cp => cp.TaskChecklistItemId == itemId);

        return new TaskChecklistProgressDTO
        {
            Id = progress.Id,
            WorkTaskId = progress.WorkTaskId,
            TaskChecklistItemId = progress.TaskChecklistItemId,
            ChecklistItemDescription = progress.TaskChecklistItem?.Description ?? "",
            Sequence = progress.TaskChecklistItem?.Sequence ?? 0,
            IsRequired = progress.TaskChecklistItem?.IsRequired ?? false,
            IsCompleted = progress.IsCompleted,
            CompletedAt = progress.CompletedAt,
            CompletedBy = progress.CompletedBy,
            CompletedByName = progress.CompletedByEmployee?.FullName,
            CompletionNotes = progress.CompletionNotes
        };
    }

    public async Task<TaskChecklistProgressDTO> MarkChecklistItemIncompleteAsync(Guid taskId, Guid itemId)
    {
        var task = await _taskRepository.GetByIdAsync(taskId);
        if (task == null)
        {
            throw new KeyNotFoundException($"Task with ID {taskId} not found");
        }

        var progress = task.ChecklistProgress.FirstOrDefault(cp => cp.TaskChecklistItemId == itemId);
        if (progress == null)
        {
            throw new KeyNotFoundException($"Checklist item {itemId} not found in task {taskId}");
        }

        progress.IsCompleted = false;
        progress.CompletedAt = null;
        progress.CompletedBy = null;
        progress.CompletionNotes = null;
        progress.UpdatedAt = DateTime.UtcNow;

        await _taskRepository.UpdateAsync(task);
        await _unitOfWork.SaveChangesAsync();

        // Reload to get updated navigation properties
        task = await _taskRepository.GetByIdAsync(taskId);
        progress = task!.ChecklistProgress.First(cp => cp.TaskChecklistItemId == itemId);

        return new TaskChecklistProgressDTO
        {
            Id = progress.Id,
            WorkTaskId = progress.WorkTaskId,
            TaskChecklistItemId = progress.TaskChecklistItemId,
            ChecklistItemDescription = progress.TaskChecklistItem?.Description ?? "",
            Sequence = progress.TaskChecklistItem?.Sequence ?? 0,
            IsRequired = progress.TaskChecklistItem?.IsRequired ?? false,
            IsCompleted = progress.IsCompleted,
            CompletedAt = progress.CompletedAt,
            CompletedBy = progress.CompletedBy,
            CompletedByName = progress.CompletedByEmployee?.FullName,
            CompletionNotes = progress.CompletionNotes
        };
    }

    private static WorkTaskDTO MapToDto(WorkTask task)
    {
        return new WorkTaskDTO
        {
            Id = task.Id,
            TaskDescription = task.TaskDescription,
            WorkAreaId = task.WorkAreaId,
            WorkAreaName = task.WorkArea?.Name,
            TeamId = task.TeamId,
            TeamName = task.Team?.Name,
            EmployeeId = task.EmployeeId,
            EmployeeName = task.Employee?.FullName,
            TaskStatusId = task.TaskStatusId,
            TaskStatusName = task.TaskStatus?.Name ?? "",
            TaskTemplateId = task.TaskTemplateId,
            TaskTemplateName = task.TaskTemplate?.Name,
            ScheduledDate = task.ScheduledDate,
            StartDate = task.StartDate,
            CompletedDate = task.CompletedDate,
            EstimatedHours = task.EstimatedHours,
            ActualHours = task.ActualHours,
            Notes = task.Notes,
            ChecklistProgress = task.ChecklistProgress.Select(cp => new TaskChecklistProgressDTO
            {
                Id = cp.Id,
                WorkTaskId = cp.WorkTaskId,
                TaskChecklistItemId = cp.TaskChecklistItemId,
                ChecklistItemDescription = cp.TaskChecklistItem?.Description ?? "",
                Sequence = cp.TaskChecklistItem?.Sequence ?? 0,
                IsRequired = cp.TaskChecklistItem?.IsRequired ?? false,
                IsCompleted = cp.IsCompleted,
                CompletedAt = cp.CompletedAt,
                CompletedBy = cp.CompletedBy,
                CompletedByName = cp.CompletedByEmployee?.FullName,
                CompletionNotes = cp.CompletionNotes
            }).ToList(),
            InventoryAllocations = task.InventoryAllocations.Select(ia => new TaskInventoryAllocationDTO
            {
                Id = ia.Id,
                WorkTaskId = ia.WorkTaskId,
                InventoryItemId = ia.InventoryItemId,
                InventoryItemName = ia.InventoryItem?.Name ?? "",
                InventoryItemCode = ia.InventoryItem?.SKU,
                UnitOfMeasure = ia.InventoryItem?.UnitOfMeasure?.Name,
                TaskTemplateInventoryItemId = ia.TaskTemplateInventoryItemId,
                PlannedQuantity = ia.PlannedQuantity,
                ActualQuantity = ia.ActualQuantity,
                AllocationMethod = ia.AllocationMethod,
                TeamMemberCount = ia.TeamMemberCount,
                Notes = ia.Notes,
                IsIssued = ia.IsIssued,
                InventoryIssueId = ia.InventoryIssueId
            }).ToList(),
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }
}

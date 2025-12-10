namespace FarmSync.Application.DTOs.HR;

public class WorkTaskDTO
{
    public Guid Id { get; set; }
    public string TaskDescription { get; set; } = string.Empty;
    public Guid? WorkAreaId { get; set; }
    public string? WorkAreaName { get; set; }
    public Guid? TeamId { get; set; }
    public string? TeamName { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public Guid TaskStatusId { get; set; }
    public string TaskStatusName { get; set; } = string.Empty;
    public Guid? TaskTemplateId { get; set; }
    public string? TaskTemplateName { get; set; }
    public DateOnly? ScheduledDate { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public string? Notes { get; set; }
    public List<InventoryIssueDTO> InventoryIssues { get; set; } = new();
    public List<EquipmentIssueDTO> EquipmentIssues { get; set; } = new();
    public List<TaskChecklistProgressDTO> ChecklistProgress { get; set; } = new();
    public List<TaskInventoryAllocationDTO> InventoryAllocations { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateWorkTaskDTO
{
    public string TaskDescription { get; set; } = string.Empty;
    public Guid? WorkAreaId { get; set; }
    public Guid? TeamId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid TaskStatusId { get; set; }
    public DateOnly? ScheduledDate { get; set; }
    public decimal? EstimatedHours { get; set; }
    public string? Notes { get; set; }
}

public class UpdateWorkTaskDTO
{
    public string TaskDescription { get; set; } = string.Empty;
    public Guid? WorkAreaId { get; set; }
    public Guid? TeamId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid TaskStatusId { get; set; }
    public DateOnly? ScheduledDate { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }
    public string? Notes { get; set; }
}

public class CreateTaskFromTemplateDTO
{
    public Guid TaskTemplateId { get; set; }
    public Guid? WorkAreaId { get; set; }
    public Guid? TeamId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid TaskStatusId { get; set; }
    public DateOnly? ScheduledDate { get; set; }
    public string? AdditionalNotes { get; set; }
    public List<CreateTaskInventoryAllocationDTO> InventoryAllocations { get; set; } = new();
}

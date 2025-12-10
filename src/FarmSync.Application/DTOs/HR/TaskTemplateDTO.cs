namespace FarmSync.Application.DTOs.HR;

public class TaskTemplateDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal? EstimatedHours { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
    public int? RecurrenceInterval { get; set; }
    public string? Instructions { get; set; }
    public bool IsActive { get; set; }
    public List<TaskChecklistItemDTO> ChecklistItems { get; set; } = new();
    public List<TaskTemplateInventoryItemDTO> InventoryItems { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class TaskChecklistItemDTO
{
    public Guid Id { get; set; }
    public Guid TaskTemplateId { get; set; }
    public int Sequence { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; }
    public string? Notes { get; set; }
}

public class CreateTaskTemplateDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal? EstimatedHours { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
    public int? RecurrenceInterval { get; set; }
    public string? Instructions { get; set; }
    public List<CreateChecklistItemDTO> ChecklistItems { get; set; } = new();
    public List<CreateTemplateInventoryItemDTO> InventoryItems { get; set; } = new();
}

public class CreateChecklistItemDTO
{
    public int Sequence { get; set; }
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
    public string? Notes { get; set; }
}

public class UpdateTaskTemplateDTO
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; }
    public decimal? EstimatedHours { get; set; }
    public bool IsRecurring { get; set; }
    public string? RecurrencePattern { get; set; }
    public int? RecurrenceInterval { get; set; }
    public string? Instructions { get; set; }
    public bool IsActive { get; set; }
    public List<CreateChecklistItemDTO> ChecklistItems { get; set; } = new();
    public List<CreateTemplateInventoryItemDTO> InventoryItems { get; set; } = new();
}

public class TaskChecklistProgressDTO
{
    public Guid Id { get; set; }
    public Guid WorkTaskId { get; set; }
    public Guid TaskChecklistItemId { get; set; }
    public string ChecklistItemDescription { get; set; } = string.Empty;
    public int Sequence { get; set; }
    public bool IsRequired { get; set; }
    public bool IsCompleted { get; set; }
    public DateTime? CompletedAt { get; set; }
    public Guid? CompletedBy { get; set; }
    public string? CompletedByName { get; set; }
    public string? CompletionNotes { get; set; }
}

public class TaskTemplateInventoryItemDTO
{
    public Guid Id { get; set; }
    public Guid TaskTemplateId { get; set; }
    public Guid InventoryItemId { get; set; }
    public string InventoryItemName { get; set; } = string.Empty;
    public string? InventoryItemCode { get; set; }
    public string? UnitOfMeasure { get; set; }
    public decimal QuantityPerUnit { get; set; }
    public string AllocationMethod { get; set; } = "Custom"; // PerTask, PerTeamMember, Custom
    public string? Notes { get; set; }
    public int Sequence { get; set; }
}

public class CreateTemplateInventoryItemDTO
{
    public Guid InventoryItemId { get; set; }
    public decimal QuantityPerUnit { get; set; }
    public string AllocationMethod { get; set; } = "Custom"; // PerTask, PerTeamMember, Custom
    public string? Notes { get; set; }
    public int Sequence { get; set; }
}

public class TaskInventoryAllocationDTO
{
    public Guid Id { get; set; }
    public Guid WorkTaskId { get; set; }
    public Guid InventoryItemId { get; set; }
    public string InventoryItemName { get; set; } = string.Empty;
    public string? InventoryItemCode { get; set; }
    public string? UnitOfMeasure { get; set; }
    public Guid? TaskTemplateInventoryItemId { get; set; }
    public decimal PlannedQuantity { get; set; }
    public decimal? ActualQuantity { get; set; }
    public string AllocationMethod { get; set; } = "Custom";
    public int? TeamMemberCount { get; set; }
    public string? Notes { get; set; }
    public bool IsIssued { get; set; }
    public Guid? InventoryIssueId { get; set; }
}

public class CreateTaskInventoryAllocationDTO
{
    public Guid InventoryItemId { get; set; }
    public Guid? TaskTemplateInventoryItemId { get; set; }
    public decimal PlannedQuantity { get; set; }
    public string AllocationMethod { get; set; } = "Custom";
    public int? TeamMemberCount { get; set; }
    public string? Notes { get; set; }
}


using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class TaskInventoryAllocation : BaseEntity
{
    public Guid WorkTaskId { get; set; }
    public Guid InventoryItemId { get; set; }
    public Guid? TaskTemplateInventoryItemId { get; set; } // Link to template item if from template
    public decimal PlannedQuantity { get; set; }
    public decimal? ActualQuantity { get; set; }
    public string AllocationMethod { get; set; } = "Custom"; // PerTask, PerTeamMember, Custom
    public int? TeamMemberCount { get; set; } // Used when AllocationMethod is PerTeamMember
    public string? Notes { get; set; }
    public bool IsIssued { get; set; } = false; // Has inventory been issued
    public Guid? InventoryIssueId { get; set; } // Link to actual inventory transaction

    // Navigation properties
    public virtual WorkTask WorkTask { get; set; } = null!;
    public virtual Inventory.InventoryItem InventoryItem { get; set; } = null!;
    public virtual TaskTemplateInventoryItem? TaskTemplateInventoryItem { get; set; }
    public virtual InventoryIssue? InventoryIssue { get; set; }
}

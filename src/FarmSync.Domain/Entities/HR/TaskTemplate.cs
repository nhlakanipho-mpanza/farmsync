using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class TaskTemplate : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Category { get; set; } // Planting, Harvesting, Maintenance, etc.
    public decimal? EstimatedHours { get; set; }
    public bool IsRecurring { get; set; } = false;
    public string? RecurrencePattern { get; set; } // Daily, Weekly, Monthly, Seasonal
    public int? RecurrenceInterval { get; set; } // Every X days/weeks/months
    public string? Instructions { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<TaskChecklistItem> ChecklistItems { get; set; } = new List<TaskChecklistItem>();
    public virtual ICollection<TaskTemplateInventoryItem> InventoryItems { get; set; } = new List<TaskTemplateInventoryItem>();
    public virtual ICollection<WorkTask> WorkTasks { get; set; } = new List<WorkTask>();
}

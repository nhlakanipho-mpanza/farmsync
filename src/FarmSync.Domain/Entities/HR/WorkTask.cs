using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class WorkTask : BaseEntity
{
    public string TaskDescription { get; set; } = string.Empty;
    public Guid? WorkAreaId { get; set; }
    public Guid? TeamId { get; set; }
    public Guid? EmployeeId { get; set; } // For individual assignments
    public Guid TaskStatusId { get; set; }
    public DateOnly? ScheduledDate { get; set; }
    public DateOnly? StartDate { get; set; }
    public DateOnly? CompletedDate { get; set; }
    public string? Notes { get; set; }
    public decimal? EstimatedHours { get; set; }
    public decimal? ActualHours { get; set; }

    // Navigation properties
    public virtual WorkArea? WorkArea { get; set; }
    public virtual Team? Team { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual TaskStatus TaskStatus { get; set; } = null!;
    public virtual ICollection<InventoryIssue> InventoryIssues { get; set; } = new List<InventoryIssue>();
    public virtual ICollection<EquipmentIssue> EquipmentIssues { get; set; } = new List<EquipmentIssue>();
}

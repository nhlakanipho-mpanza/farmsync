using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class TaskStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Pending, InProgress, Completed, Cancelled
    public string? Description { get; set; }
    public string? ColorCode { get; set; } // For UI badge colors
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<WorkTask> WorkTasks { get; set; } = new List<WorkTask>();
}

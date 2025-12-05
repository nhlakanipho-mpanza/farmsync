using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Fleet;

public class TaskStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<TransportTask> TransportTasks { get; set; } = new List<TransportTask>();
}

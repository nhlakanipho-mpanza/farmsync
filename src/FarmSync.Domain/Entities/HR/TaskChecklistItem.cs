using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class TaskChecklistItem : BaseEntity
{
    public Guid TaskTemplateId { get; set; }
    public int Sequence { get; set; } // Order of items
    public string Description { get; set; } = string.Empty;
    public bool IsRequired { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual TaskTemplate TaskTemplate { get; set; } = null!;
    public virtual ICollection<TaskChecklistProgress> ChecklistProgress { get; set; } = new List<TaskChecklistProgress>();
}

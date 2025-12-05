using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class WorkArea : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? Size { get; set; } // In hectares or acres
    public string? SizeUnit { get; set; }
    public string? Location { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<WorkTask> Tasks { get; set; } = new List<WorkTask>();
}

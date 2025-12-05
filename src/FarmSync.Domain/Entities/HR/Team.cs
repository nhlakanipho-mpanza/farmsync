using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class Team : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public Guid? TeamLeaderId { get; set; }
    public Guid? TeamTypeId { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Employee? TeamLeader { get; set; }
    public virtual TeamType? TeamType { get; set; }
    public virtual ICollection<TeamMember> Members { get; set; } = new List<TeamMember>();
    public virtual ICollection<WorkTask> AssignedTasks { get; set; } = new List<WorkTask>();
}

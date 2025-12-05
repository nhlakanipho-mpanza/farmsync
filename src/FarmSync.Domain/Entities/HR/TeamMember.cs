using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class TeamMember : BaseEntity
{
    public Guid TeamId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly AssignedDate { get; set; }
    public DateOnly? EndDate { get; set; } // Null for permanent assignments
    public bool IsPermanent { get; set; } = true;
    public bool IsActive { get; set; } = true;
    public string? Notes { get; set; }

    // Navigation properties
    public virtual Team Team { get; set; } = null!;
    public virtual Employee Employee { get; set; } = null!;
}

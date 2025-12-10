using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.ReferenceData;

public class LeaveType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool RequiresDocument { get; set; } = false; // e.g., Sick leave requires doctor's note
    public int DefaultDaysPerYear { get; set; } = 0; // e.g., Annual leave: 15 days
    public bool IsActive { get; set; } = true;
}

using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class EmergencyContact : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string? AlternateNumber { get; set; }
    public string? Address { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}

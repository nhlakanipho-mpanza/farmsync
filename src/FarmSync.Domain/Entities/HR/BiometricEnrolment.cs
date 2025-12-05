using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class BiometricEnrolment : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public string BiometricId { get; set; } = string.Empty; // Fingerprint device ID
    public DateTime EnrolledAt { get; set; }
    public string? EnrolledBy { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
}

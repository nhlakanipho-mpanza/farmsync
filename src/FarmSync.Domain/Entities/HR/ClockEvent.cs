using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class ClockEvent : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Guid? TeamId { get; set; } // Team they were part of during this shift
    public DateTime EventTime { get; set; }
    public string EventType { get; set; } = string.Empty; // ClockIn, ClockOut, Break
    public string? BiometricId { get; set; } // Fingerprint verification
    public string? Notes { get; set; }
    public string? Location { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual Team? Team { get; set; }
}

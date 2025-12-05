using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Entities.Fleet;

public class DriverAssignment : BaseEntity
{
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsPrimary { get; set; } = false;
    public string? Notes { get; set; }
    
    public string AssignmentType { get; set; } = "Primary"; // "Primary", "Temporary", "Pool"
    
    // Indicates if this is the current/active assignment
    public bool IsCurrentAssignment => !EndDate.HasValue || EndDate.Value > DateTime.UtcNow;
    
    public Guid? AssignedById { get; set; }
    public virtual Employee? AssignedBy { get; set; }

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }

    // Navigation Properties
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual Employee Driver { get; set; } = null!;
}

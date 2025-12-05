using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Entities.Fleet;

public class SpeedingEvent : BaseEntity
{
    public DateTime Timestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double Speed { get; set; }
    public double SpeedLimit { get; set; }
    public string? Location { get; set; }
    public bool IsAcknowledged { get; set; } = false;
    public DateTime? AcknowledgedDate { get; set; }
    public string? AcknowledgedBy { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }

    // Navigation Properties
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual Employee Driver { get; set; } = null!;
}

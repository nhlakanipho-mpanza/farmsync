using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Entities.Fleet;

public class TripLog : BaseEntity
{
    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }
    public int StartOdometer { get; set; }
    public int? EndOdometer { get; set; }
    public int? DistanceTraveled { get; set; }
    public string? StartLocation { get; set; }
    public string? EndLocation { get; set; }
    public double? StartLatitude { get; set; }
    public double? StartLongitude { get; set; }
    public double? EndLatitude { get; set; }
    public double? EndLongitude { get; set; }
    public string? Purpose { get; set; }
    public string? Notes { get; set; }
    public bool IsCompleted { get; set; } = false;
    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public Guid? TransportTaskId { get; set; }

    // Navigation Properties
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual Employee Driver { get; set; } = null!;
    public virtual TransportTask? TransportTask { get; set; }
    public virtual ICollection<GPSLocation> GPSLocations { get; set; } = new List<GPSLocation>();
}

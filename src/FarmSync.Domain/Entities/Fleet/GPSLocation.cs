using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Fleet;

public class GPSLocation : BaseEntity
{
    public DateTime Timestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public double? Speed { get; set; }
    public double? Heading { get; set; }
    public int? Odometer { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public Guid? TripLogId { get; set; }

    // Navigation Properties
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual TripLog? TripLog { get; set; }
}

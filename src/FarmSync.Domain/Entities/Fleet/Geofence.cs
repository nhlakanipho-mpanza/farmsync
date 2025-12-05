using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Fleet;

public class Geofence : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string GeofenceType { get; set; } = "Circle"; // Circle, Polygon
    public double? CenterLatitude { get; set; }
    public double? CenterLongitude { get; set; }
    public double? Radius { get; set; } // In meters
    public string? PolygonCoordinates { get; set; } // JSON array of lat/lng points
    public bool AlertOnEntry { get; set; } = false;
    public bool AlertOnExit { get; set; } = true;
    public bool IsActive { get; set; } = true;
}

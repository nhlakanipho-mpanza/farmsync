namespace FarmSync.Application.DTOs.Fleet;

public class TripLogDTO
{
    public Guid Id { get; set; }
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
    public bool IsCompleted { get; set; }
    public bool IsActive { get; set; }

    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public Guid? TransportTaskId { get; set; }

    public string? VehicleRegistration { get; set; }
    public string? DriverName { get; set; }
    public string? TaskNumber { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class CreateTripLogDTO
{
    public DateTime StartTime { get; set; }
    public int StartOdometer { get; set; }
    public string? StartLocation { get; set; }
    public double? StartLatitude { get; set; }
    public double? StartLongitude { get; set; }
    public string? Purpose { get; set; }
    public string? Notes { get; set; }

    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public Guid? TransportTaskId { get; set; }
}

public class UpdateTripLogDTO
{
    public DateTime? EndTime { get; set; }
    public int? EndOdometer { get; set; }
    public string? EndLocation { get; set; }
    public double? EndLatitude { get; set; }
    public double? EndLongitude { get; set; }
    public string? Notes { get; set; }
    public bool IsCompleted { get; set; }
}

namespace FarmSync.Application.DTOs.Fleet;

public class GPSLocationDTO
{
    public Guid Id { get; set; }
    public DateTime Timestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public double? Speed { get; set; }
    public double? Heading { get; set; }
    public int? Odometer { get; set; }

    public Guid VehicleId { get; set; }
    public Guid? TripLogId { get; set; }

    public string? VehicleRegistration { get; set; }
}

public class CreateGPSLocationDTO
{
    public DateTime Timestamp { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }
    public double? Altitude { get; set; }
    public double? Speed { get; set; }
    public double? Heading { get; set; }
    public int? Odometer { get; set; }

    public Guid VehicleId { get; set; }
    public Guid? TripLogId { get; set; }
}

public class TransportTaskDTO
{
    public Guid Id { get; set; }
    public string TaskNumber { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public string? PickupLocation { get; set; }
    public string? DeliveryLocation { get; set; }
    public double? PickupLatitude { get; set; }
    public double? PickupLongitude { get; set; }
    public double? DeliveryLatitude { get; set; }
    public double? DeliveryLongitude { get; set; }
    public string? CargoDescription { get; set; }
    public decimal? CargoWeight { get; set; }
    public string? SpecialInstructions { get; set; }
    public string? Notes { get; set; }

    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public Guid TaskStatusId { get; set; }
    public Guid? AssignedById { get; set; }

    public string? VehicleRegistration { get; set; }
    public string? DriverName { get; set; }
    public string? TaskStatusName { get; set; }
    public string? AssignedByName { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class CreateTransportTaskDTO
{
    public string Description { get; set; } = string.Empty;
    public DateTime ScheduledDate { get; set; }
    public string? PickupLocation { get; set; }
    public string? DeliveryLocation { get; set; }
    public double? PickupLatitude { get; set; }
    public double? PickupLongitude { get; set; }
    public double? DeliveryLatitude { get; set; }
    public double? DeliveryLongitude { get; set; }
    public string? CargoDescription { get; set; }
    public decimal? CargoWeight { get; set; }
    public string? SpecialInstructions { get; set; }

    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public Guid TaskStatusId { get; set; }
}

public class UpdateTransportTaskDTO
{
    public DateTime? CompletedDate { get; set; }
    public string? Notes { get; set; }
    public Guid TaskStatusId { get; set; }
}

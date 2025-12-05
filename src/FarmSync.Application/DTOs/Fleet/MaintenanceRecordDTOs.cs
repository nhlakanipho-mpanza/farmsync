namespace FarmSync.Application.DTOs.Fleet;

public class MaintenanceRecordDTO
{
    public Guid Id { get; set; }
    public DateTime ScheduledDate { get; set; }
    public DateTime? CompletedDate { get; set; }
    public int OdometerReading { get; set; }
    public decimal? EngineHours { get; set; }
    public string? Description { get; set; }
    public string? PartsReplaced { get; set; }
    public decimal? LaborCost { get; set; }
    public decimal? PartsCost { get; set; }
    public decimal TotalCost { get; set; }
    public string? MechanicNotes { get; set; }
    public int? NextServiceOdometer { get; set; }
    public decimal? NextServiceHours { get; set; }
    public bool IsCompleted { get; set; }
    public bool IsActive { get; set; }

    public Guid VehicleId { get; set; }
    public Guid MaintenanceTypeId { get; set; }
    public Guid? PerformedById { get; set; }

    public string? VehicleRegistration { get; set; }
    public string? MaintenanceTypeName { get; set; }
    public string? PerformedByName { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class CreateMaintenanceRecordDTO
{
    public DateTime ScheduledDate { get; set; }
    public int OdometerReading { get; set; }
    public decimal? EngineHours { get; set; }
    public string? Description { get; set; }

    public Guid VehicleId { get; set; }
    public Guid MaintenanceTypeId { get; set; }
}

public class UpdateMaintenanceRecordDTO
{
    public DateTime? CompletedDate { get; set; }
    public string? PartsReplaced { get; set; }
    public decimal? LaborCost { get; set; }
    public decimal? PartsCost { get; set; }
    public decimal TotalCost { get; set; }
    public string? MechanicNotes { get; set; }
    public int? NextServiceOdometer { get; set; }
    public decimal? NextServiceHours { get; set; }
    public bool IsCompleted { get; set; }
    public Guid? PerformedById { get; set; }
}

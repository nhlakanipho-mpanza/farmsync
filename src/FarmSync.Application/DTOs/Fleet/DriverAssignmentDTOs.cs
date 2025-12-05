namespace FarmSync.Application.DTOs.Fleet;

public class DriverAssignmentDTO
{
    public Guid Id { get; set; }
    public Guid VehicleId { get; set; }
    public string? VehicleRegistration { get; set; }
    public string? VehicleMake { get; set; }
    public string? VehicleModel { get; set; }
    
    public Guid DriverId { get; set; }
    public string? DriverName { get; set; }
    public string? DriverEmployeeNumber { get; set; }
    
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public string AssignmentType { get; set; } = null!;
    public bool IsPrimary { get; set; }
    public string? Notes { get; set; }
    
    public bool IsCurrentAssignment { get; set; }
    
    public Guid? AssignedById { get; set; }
    public string? AssignedByName { get; set; }
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateDriverAssignmentDTO
{
    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public string AssignmentType { get; set; } = "Primary"; // "Primary", "Temporary", "Pool"
    public bool IsPrimary { get; set; } = true;
    public string? Notes { get; set; }
}

public class UpdateDriverAssignmentDTO
{
    public DateTime? EndDate { get; set; }
    public string? Notes { get; set; }
}

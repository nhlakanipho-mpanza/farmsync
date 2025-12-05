namespace FarmSync.Application.DTOs.Fleet;

public class IncidentReportDTO
{
    public Guid Id { get; set; }
    public DateTime IncidentDate { get; set; }
    public string IncidentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Severity { get; set; }
    public string? PhotoUrl { get; set; }
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public string Status { get; set; } = "Reported";
    public DateTime? ResolvedDate { get; set; }
    public string? ResolutionNotes { get; set; }
    public bool IsActive { get; set; }

    public Guid VehicleId { get; set; }
    public Guid ReportedById { get; set; }
    public Guid? AssignedToId { get; set; }

    public string? VehicleRegistration { get; set; }
    public string? ReportedByName { get; set; }
    public string? AssignedToName { get; set; }

    public DateTime CreatedAt { get; set; }
}

public class CreateIncidentReportDTO
{
    public DateTime IncidentDate { get; set; }
    public string IncidentType { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Severity { get; set; }
    public string? PhotoUrl { get; set; }

    public Guid VehicleId { get; set; }
    public Guid ReportedById { get; set; }
}

public class UpdateIncidentReportDTO
{
    public string Status { get; set; } = "Reported";
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public DateTime? ResolvedDate { get; set; }
    public string? ResolutionNotes { get; set; }
    public Guid? AssignedToId { get; set; }
}

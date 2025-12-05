using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Entities.Fleet;

public class IncidentReport : BaseEntity
{
    public DateTime IncidentDate { get; set; }
    public string IncidentType { get; set; } = string.Empty; // Breakdown, Accident, Damage, Theft
    public string Description { get; set; } = string.Empty;
    public string? Location { get; set; }
    public double? Latitude { get; set; }
    public double? Longitude { get; set; }
    public string? Severity { get; set; } // Minor, Moderate, Severe
    public string? PhotoUrl { get; set; }
    public decimal? EstimatedCost { get; set; }
    public decimal? ActualCost { get; set; }
    public string Status { get; set; } = "Reported"; // Reported, Diagnosed, Repairing, Resolved
    public DateTime? ResolvedDate { get; set; }
    public string? ResolutionNotes { get; set; }
    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public Guid ReportedById { get; set; }
    public Guid? AssignedToId { get; set; }

    // Navigation Properties
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual Employee ReportedBy { get; set; } = null!;
    public virtual Employee? AssignedTo { get; set; }
}

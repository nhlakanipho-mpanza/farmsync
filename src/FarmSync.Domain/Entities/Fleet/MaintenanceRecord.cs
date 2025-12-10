using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Entities.Fleet;

public class MaintenanceRecord : BaseEntity
{
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
    public bool IsCompleted { get; set; } = false;
    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public Guid MaintenanceTypeId { get; set; }
    public Guid? PerformedById { get; set; }

    // Navigation Properties
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual MaintenanceType MaintenanceType { get; set; } = null!;
    public virtual Employee? PerformedBy { get; set; }
    public virtual ICollection<Documents.Document> Documents { get; set; } = new List<Documents.Document>();
}

using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Entities.Fleet;

public class TransportTask : BaseEntity
{
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
    public bool IsActive { get; set; } = true;

    // Foreign Keys
    public Guid VehicleId { get; set; }
    public Guid DriverId { get; set; }
    public Guid TaskStatusId { get; set; }
    public Guid? AssignedById { get; set; }

    // Navigation Properties
    public virtual Vehicle Vehicle { get; set; } = null!;
    public virtual Employee Driver { get; set; } = null!;
    public virtual TaskStatus TaskStatus { get; set; } = null!;
    public virtual Employee? AssignedBy { get; set; }
    public virtual ICollection<TripLog> TripLogs { get; set; } = new List<TripLog>();
}

using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.ReferenceData;

namespace FarmSync.Domain.Entities.Inventory;

public class EquipmentMaintenanceRecord : BaseEntity
{
    public Guid EquipmentId { get; set; }
    public Equipment Equipment { get; set; } = null!;
    
    public Guid MaintenanceTypeId { get; set; }
    public MaintenanceType MaintenanceType { get; set; } = null!;
    
    public DateTime MaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    
    public string? Description { get; set; }
    public decimal? Cost { get; set; }
    public string? PerformedBy { get; set; }
    public string? Notes { get; set; }
}

using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.ReferenceData;

namespace FarmSync.Domain.Entities.Inventory;

public class Equipment : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public string? Manufacturer { get; set; }
    
    public Guid ConditionId { get; set; }
    public EquipmentCondition Condition { get; set; } = null!;
    
    public Guid? LocationId { get; set; }
    public InventoryLocation? Location { get; set; }
    
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDue { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<EquipmentMaintenanceRecord> MaintenanceRecords { get; set; } = new List<EquipmentMaintenanceRecord>();
}

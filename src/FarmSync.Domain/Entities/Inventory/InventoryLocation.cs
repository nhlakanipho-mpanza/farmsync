using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Inventory;

public class InventoryLocation : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();
    public ICollection<InventoryTransaction> Transactions { get; set; } = new List<InventoryTransaction>();
}

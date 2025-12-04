using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.ReferenceData;

namespace FarmSync.Domain.Entities.Inventory;

public class InventoryItem : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SKU { get; set; }
    
    public Guid CategoryId { get; set; }
    public InventoryCategory Category { get; set; } = null!;
    
    public Guid TypeId { get; set; }
    public InventoryType Type { get; set; } = null!;
    
    public Guid UnitOfMeasureId { get; set; }
    public UnitOfMeasure UnitOfMeasure { get; set; } = null!;
    
    public decimal MinimumStockLevel { get; set; }
    public decimal ReorderLevel { get; set; }
    public decimal? UnitPrice { get; set; }  // Standard/List price (for reference)
    public decimal AverageUnitCost { get; set; }  // Weighted average cost (for accounting)
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public ICollection<StockLevel> StockLevels { get; set; } = new List<StockLevel>();
    public ICollection<InventoryTransaction> Transactions { get; set; } = new List<InventoryTransaction>();
}

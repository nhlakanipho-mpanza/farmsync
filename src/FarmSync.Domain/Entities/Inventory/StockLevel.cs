using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Inventory;

public class StockLevel : BaseEntity
{
    public Guid InventoryItemId { get; set; }
    public InventoryItem InventoryItem { get; set; } = null!;
    
    public Guid LocationId { get; set; }
    public InventoryLocation Location { get; set; } = null!;
    
    public decimal Quantity { get; set; }
    public decimal? ReservedQuantity { get; set; }
    public decimal AvailableQuantity => Quantity - (ReservedQuantity ?? 0);
}

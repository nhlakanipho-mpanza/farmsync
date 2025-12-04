using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Domain.Entities.Procurement;

public class PurchaseOrderItem : BaseEntity
{
    public Guid PurchaseOrderId { get; set; }
    public Guid InventoryItemId { get; set; }
    public decimal OrderedQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
    public virtual InventoryItem InventoryItem { get; set; } = null!;
    public virtual ICollection<GoodsReceivedItem> GoodsReceivedItems { get; set; } = new List<GoodsReceivedItem>();
}

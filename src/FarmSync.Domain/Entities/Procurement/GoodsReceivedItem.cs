using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Procurement;

public class GoodsReceivedItem : BaseEntity
{
    public Guid GoodsReceivedId { get; set; }
    public Guid PurchaseOrderItemId { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal QuantityDamaged { get; set; }
    public decimal QuantityShortfall { get; set; }
    public string Condition { get; set; } = "Good";
    public string? Notes { get; set; }

    // Navigation properties
    public virtual GoodsReceived GoodsReceived { get; set; } = null!;
    public virtual PurchaseOrderItem PurchaseOrderItem { get; set; } = null!;
}

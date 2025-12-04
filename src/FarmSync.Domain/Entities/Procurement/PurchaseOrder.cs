using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Procurement;

public class PurchaseOrder : BaseEntity
{
    public string PONumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public POStatus Status { get; set; } = POStatus.Created;
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Navigation properties
    public virtual Supplier Supplier { get; set; } = null!;
    public virtual ICollection<PurchaseOrderItem> Items { get; set; } = new List<PurchaseOrderItem>();
    public virtual ICollection<GoodsReceived> GoodsReceivedRecords { get; set; } = new List<GoodsReceived>();
}

public enum POStatus
{
    Created = 0,
    Approved = 1,
    PartiallyReceived = 2,
    FullyReceived = 3,
    Closed = 4,
    ClosedWithIssues = 5,
    Cancelled = 6
}

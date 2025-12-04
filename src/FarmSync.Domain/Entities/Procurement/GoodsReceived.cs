using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Procurement;

public class GoodsReceived : BaseEntity
{
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid PurchaseOrderId { get; set; }
    public DateTime ReceivedDate { get; set; }
    public Guid ReceivedBy { get; set; }
    public GRStatus Status { get; set; } = GRStatus.Pending;
    public bool HasDiscrepancies { get; set; }
    public string? DiscrepancyNotes { get; set; }
    public bool IsFinalReceipt { get; set; }
    
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }

    // Navigation properties
    public virtual PurchaseOrder PurchaseOrder { get; set; } = null!;
    public virtual ICollection<GoodsReceivedItem> Items { get; set; } = new List<GoodsReceivedItem>();
}

public enum GRStatus
{
    Pending = 0,
    Approved = 1,
    Completed = 2,
    Rejected = 3
}

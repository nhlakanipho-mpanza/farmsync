using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.ReferenceData;

namespace FarmSync.Domain.Entities.Inventory;

public class InventoryTransaction : BaseEntity
{
    public Guid InventoryItemId { get; set; }
    public InventoryItem InventoryItem { get; set; } = null!;
    
    public Guid LocationId { get; set; }
    public InventoryLocation Location { get; set; } = null!;
    
    public Guid StatusId { get; set; }
    public TransactionStatus Status { get; set; } = null!;
    
    public string TransactionType { get; set; } = string.Empty; // Receipt, Issue, Adjustment, Transfer
    public decimal Quantity { get; set; }
    public decimal? UnitCost { get; set; }
    public decimal? TotalCost { get; set; }
    
    public string? ReferenceNumber { get; set; }
    public string? Notes { get; set; }
    
    public DateTime TransactionDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
}

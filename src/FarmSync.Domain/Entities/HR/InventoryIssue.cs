using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Domain.Entities.HR;

public class InventoryIssue : BaseEntity
{
    public string IssueNumber { get; set; } = string.Empty;
    public Guid InventoryItemId { get; set; }
    public decimal Quantity { get; set; }
    public Guid? WorkTaskId { get; set; }
    public Guid? TeamId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid IssueStatusId { get; set; }
    public string? Purpose { get; set; }
    public DateTime IssuedDate { get; set; }
    public string IssuedBy { get; set; } = string.Empty; // Team leader who initiated
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; } // Operations manager
    public decimal? ReturnedQuantity { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public string? Notes { get; set; }

    // Navigation properties
    public virtual InventoryItem InventoryItem { get; set; } = null!;
    public virtual WorkTask? WorkTask { get; set; }
    public virtual Team? Team { get; set; }
    public virtual Employee? Employee { get; set; }
    public virtual IssueStatus IssueStatus { get; set; } = null!;
}

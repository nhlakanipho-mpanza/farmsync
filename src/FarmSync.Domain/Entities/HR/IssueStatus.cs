using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class IssueStatus : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Pending, Approved, Issued, Returned, Cancelled
    public string? Description { get; set; }
    public string? ColorCode { get; set; } // For UI badge colors
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<InventoryIssue> InventoryIssues { get; set; } = new List<InventoryIssue>();
    public virtual ICollection<EquipmentIssue> EquipmentIssues { get; set; } = new List<EquipmentIssue>();
}

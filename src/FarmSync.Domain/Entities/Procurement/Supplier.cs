using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Procurement;

public class Supplier : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string? Address { get; set; }
    public string? TaxNumber { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<PurchaseOrder> PurchaseOrders { get; set; } = new List<PurchaseOrder>();
}

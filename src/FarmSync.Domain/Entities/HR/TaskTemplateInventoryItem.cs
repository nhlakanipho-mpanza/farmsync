using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class TaskTemplateInventoryItem : BaseEntity
{
    public Guid TaskTemplateId { get; set; }
    public Guid InventoryItemId { get; set; }
    public decimal QuantityPerUnit { get; set; } // Base quantity
    public string AllocationMethod { get; set; } = "Custom"; // PerTask, PerTeamMember, Custom
    public string? Notes { get; set; }
    public int Sequence { get; set; } // Display order

    // Navigation properties
    public virtual TaskTemplate TaskTemplate { get; set; } = null!;
    public virtual Inventory.InventoryItem InventoryItem { get; set; } = null!;
}

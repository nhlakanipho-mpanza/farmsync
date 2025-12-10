using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class BankName : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Standard Bank, FNB, Absa, Nedbank, Capitec, etc.
    public string? BranchCode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<BankDetails> BankDetails { get; set; } = new List<BankDetails>();
}

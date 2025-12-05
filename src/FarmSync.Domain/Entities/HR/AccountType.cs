using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class AccountType : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Savings, Checking, Transmission
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<BankDetails> BankDetails { get; set; } = new List<BankDetails>();
}

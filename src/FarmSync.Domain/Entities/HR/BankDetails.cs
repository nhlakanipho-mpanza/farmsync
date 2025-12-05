using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class BankDetails : BaseEntity
{
    public Guid EmployeeId { get; set; }
    public Guid? BankNameId { get; set; }
    public string AccountHolder { get; set; } = string.Empty;
    public string AccountNumber { get; set; } = string.Empty;
    public string? BranchCode { get; set; }
    public Guid? AccountTypeId { get; set; }

    // Navigation properties
    public virtual Employee Employee { get; set; } = null!;
    public virtual BankName BankName { get; set; } = null!;
    public virtual AccountType AccountType { get; set; } = null!;
}

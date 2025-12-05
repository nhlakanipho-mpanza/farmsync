namespace FarmSync.Application.DTOs.HR;

public class BankDetailsDTO
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public Guid? BankNameId { get; set; }
    public string? BankName { get; set; }
    public Guid? AccountTypeId { get; set; }
    public string? AccountType { get; set; }
    public string? BranchCode { get; set; }
}

public class CreateBankDetailsDTO
{
    public Guid EmployeeId { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public Guid? BankNameId { get; set; }
    public Guid? AccountTypeId { get; set; }
    public string? BranchCode { get; set; }
}

public class UpdateBankDetailsDTO
{
    public string AccountNumber { get; set; } = string.Empty;
    public Guid? BankNameId { get; set; }
    public Guid? AccountTypeId { get; set; }
    public string? BranchCode { get; set; }
}

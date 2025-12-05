namespace FarmSync.Application.DTOs.HR;

// Reference Data DTOs (simple read-only)
public class ReferenceDataDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class EmploymentTypeDTO : ReferenceDataDTO { }
public class RoleTypeDTO : ReferenceDataDTO { }
public class TeamTypeDTO : ReferenceDataDTO { }
public class TaskStatusDTO : ReferenceDataDTO 
{
    public string? ColorCode { get; set; }
}
public class IssueStatusDTO : ReferenceDataDTO 
{
    public string? ColorCode { get; set; }
}

public class BankNameDTO : ReferenceDataDTO 
{
    public string? BranchCode { get; set; }
}

public class AccountTypeDTO : ReferenceDataDTO { }

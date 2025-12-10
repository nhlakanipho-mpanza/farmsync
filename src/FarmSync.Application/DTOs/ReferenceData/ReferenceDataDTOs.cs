namespace FarmSync.Application.DTOs.ReferenceData;

// Base DTO for reference data reads
public class ReferenceDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

// DTO for creating reference data
public class CreateReferenceDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

// DTO for updating reference data
public class UpdateReferenceDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

// Position-specific DTOs (includes Rate and IsDriverPosition)
public class CreatePositionDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public string? Description { get; set; }
    public bool IsDriverPosition { get; set; } = false;
    public bool IsActive { get; set; } = true;
}

public class UpdatePositionDto
{
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; }
    public string? Description { get; set; }
    public bool IsDriverPosition { get; set; }
    public bool IsActive { get; set; }
}

public class PositionDto : ReferenceDto
{
    public decimal Rate { get; set; }
    public bool IsDriverPosition { get; set; }
}

// BankName-specific DTOs (includes BranchCode and Description for branch details)
public class CreateBankNameDto
{
    public string Name { get; set; } = string.Empty;
    public string? BranchCode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateBankNameDto
{
    public string Name { get; set; } = string.Empty;
    public string? BranchCode { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

public class BankNameDto : ReferenceDto
{
    public string? BranchCode { get; set; }
}

// LeaveType-specific DTOs (includes RequiresDocument and DefaultDaysPerYear)
public class CreateLeaveTypeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool RequiresDocument { get; set; } = false;
    public int DefaultDaysPerYear { get; set; } = 0;
    public bool IsActive { get; set; } = true;
}

public class UpdateLeaveTypeDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool RequiresDocument { get; set; }
    public int DefaultDaysPerYear { get; set; }
    public bool IsActive { get; set; }
}

public class LeaveTypeDto : ReferenceDto
{
    public bool RequiresDocument { get; set; }
    public int DefaultDaysPerYear { get; set; }
}

// FieldPhase-specific DTOs (includes SortOrder)
public class CreateFieldPhaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; } = true;
}

public class UpdateFieldPhaseDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; }
    public bool IsActive { get; set; }
}

public class FieldPhaseDto : ReferenceDto
{
    public int SortOrder { get; set; }
}

// DTO for replace-and-delete operation
public class ReplaceAndDeleteDto
{
    public Guid SourceId { get; set; } // ID of the entity to delete
    public Guid TargetId { get; set; } // ID of the entity to replace with
}

// DTO for usage information
public class UsageInfoDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public int UsageCount { get; set; }
    public List<string> UsedInTables { get; set; } = new();
    public bool CanDelete { get; set; }
}

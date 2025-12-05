namespace FarmSync.Application.DTOs.HR;

public class EquipmentIssueDTO
{
    public Guid Id { get; set; }
    public string IssueNumber { get; set; } = string.Empty;
    public Guid EquipmentId { get; set; }
    public string EquipmentName { get; set; } = string.Empty;
    public string? EquipmentCode { get; set; }
    public Guid? WorkTaskId { get; set; }
    public string? WorkTaskDescription { get; set; }
    public Guid? TeamId { get; set; }
    public string? TeamName { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? EmployeeName { get; set; }
    public Guid IssueStatusId { get; set; }
    public string IssueStatusName { get; set; } = string.Empty;
    public string? Purpose { get; set; }
    public DateTime IssuedDate { get; set; }
    public string IssuedBy { get; set; } = string.Empty;
    public DateTime? ApprovedDate { get; set; }
    public string? ApprovedBy { get; set; }
    public DateTime? ExpectedReturnDate { get; set; }
    public DateTime? ActualReturnDate { get; set; }
    public string? ReturnCondition { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateEquipmentIssueDTO
{
    public Guid EquipmentId { get; set; }
    public Guid? WorkTaskId { get; set; }
    public Guid? TeamId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Purpose { get; set; }
    public DateTime? ExpectedReturnDate { get; set; }
    public string? Notes { get; set; }
}

public class ApproveEquipmentIssueDTO
{
    public bool Approved { get; set; }
    public string? Notes { get; set; }
}

public class ReturnEquipmentIssueDTO
{
    public string? ReturnCondition { get; set; }
    public string? Notes { get; set; }
}

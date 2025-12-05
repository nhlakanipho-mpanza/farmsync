namespace FarmSync.Application.DTOs.HR;

public class InventoryIssueDTO
{
    public Guid Id { get; set; }
    public string IssueNumber { get; set; } = string.Empty;
    public Guid InventoryItemId { get; set; }
    public string InventoryItemName { get; set; } = string.Empty;
    public string? ItemSKU { get; set; }
    public decimal Quantity { get; set; }
    public string? UnitOfMeasure { get; set; }
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
    public decimal? ReturnedQuantity { get; set; }
    public DateTime? ReturnedDate { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateInventoryIssueDTO
{
    public Guid InventoryItemId { get; set; }
    public decimal Quantity { get; set; }
    public Guid? WorkTaskId { get; set; }
    public Guid? TeamId { get; set; }
    public Guid? EmployeeId { get; set; }
    public string? Purpose { get; set; }
    public string? Notes { get; set; }
}

public class ApproveInventoryIssueDTO
{
    public bool Approved { get; set; }
    public string? Notes { get; set; }
}

public class ReturnInventoryIssueDTO
{
    public decimal ReturnedQuantity { get; set; }
    public string? Notes { get; set; }
}

namespace FarmSync.Application.DTOs.Reports;

public class ExpenseReportDto
{
    public DateTime ExpenseDate { get; set; }
    public string Category { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public string Department { get; set; } = string.Empty;
    public string RequestedBy { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class ExpenseSummaryDto
{
    public decimal TotalExpenses { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public List<ExpenseReportDto> Expenses { get; set; } = new();
    public List<CategoryExpenseDto> CategoryBreakdown { get; set; } = new();
    public List<DepartmentExpenseDto> DepartmentBreakdown { get; set; } = new();
}

public class CategoryExpenseDto
{
    public string CategoryName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int TransactionCount { get; set; }
    public decimal Percentage { get; set; }
}

public class DepartmentExpenseDto
{
    public string DepartmentName { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public int TransactionCount { get; set; }
    public decimal Percentage { get; set; }
}

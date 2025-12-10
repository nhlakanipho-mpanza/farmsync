namespace FarmSync.Application.DTOs.Reports;

public class ReportFilterDto
{
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public Guid? SupplierId { get; set; }
    public Guid? CategoryId { get; set; }
    public Guid? EmployeeId { get; set; }
    public Guid? VehicleId { get; set; }
    public Guid? DepartmentId { get; set; }
    public string? Status { get; set; }
    public string? GroupBy { get; set; } // Daily, Weekly, Monthly, Yearly
}

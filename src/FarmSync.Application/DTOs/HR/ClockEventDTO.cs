namespace FarmSync.Application.DTOs.HR;

public class ClockEventDTO
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public Guid? TeamId { get; set; }
    public string? TeamName { get; set; }
    public DateTime EventTime { get; set; }
    public string EventType { get; set; } = string.Empty; // ClockIn, ClockOut, Break
    public string? BiometricId { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
}

public class CreateClockEventDTO
{
    public Guid EmployeeId { get; set; }
    public Guid? TeamId { get; set; }
    public DateTime EventTime { get; set; }
    public string EventType { get; set; } = string.Empty;
    public string? BiometricId { get; set; }
    public string? Location { get; set; }
    public string? Notes { get; set; }
}

public class AttendanceSummaryDTO
{
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public DateOnly Date { get; set; }
    public DateTime? ClockIn { get; set; }
    public DateTime? ClockOut { get; set; }
    public decimal? TotalHours { get; set; }
    public bool IsPresent { get; set; }
    public List<ClockEventDTO> Events { get; set; } = new();
}

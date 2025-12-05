namespace FarmSync.Application.DTOs.HR;

public class TeamMemberDTO
{
    public Guid Id { get; set; }
    public Guid TeamId { get; set; }
    public string? TeamName { get; set; }
    public Guid EmployeeId { get; set; }
    public string EmployeeName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public DateOnly AssignedDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsPermanent { get; set; }
    public string? Notes { get; set; }
}

public class CreateTeamMemberDTO
{
    public Guid TeamId { get; set; }
    public Guid EmployeeId { get; set; }
    public DateOnly AssignedDate { get; set; }
    public DateOnly? EndDate { get; set; }
    public bool IsPermanent { get; set; } = true;
    public string? Notes { get; set; }
}

public class UpdateTeamMemberDTO
{
    public DateOnly? EndDate { get; set; }
    public bool IsPermanent { get; set; }
    public string? Notes { get; set; }
}

namespace FarmSync.Application.DTOs.HR;

public class EmergencyContactDTO
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string? AlternateNumber { get; set; }
    public string? Address { get; set; }
}

public class CreateEmergencyContactDTO
{
    public Guid EmployeeId { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string Relationship { get; set; } = string.Empty;
    public string ContactNumber { get; set; } = string.Empty;
    public string? AlternateNumber { get; set; }
    public string? Address { get; set; }
}

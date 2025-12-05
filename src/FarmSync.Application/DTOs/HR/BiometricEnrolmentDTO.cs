namespace FarmSync.Application.DTOs.HR;

public class BiometricEnrolmentDTO
{
    public Guid Id { get; set; }
    public Guid EmployeeId { get; set; }
    public string BiometricId { get; set; } = string.Empty;
    public DateTime EnrolledAt { get; set; }
    public bool IsActive { get; set; }
}

public class CreateBiometricEnrolmentDTO
{
    public Guid EmployeeId { get; set; }
    public string BiometricId { get; set; } = string.Empty;
}

public class UpdateBiometricEnrolmentDTO
{
    public bool IsActive { get; set; }
}

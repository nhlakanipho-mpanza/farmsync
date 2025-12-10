namespace FarmSync.Application.DTOs.HR;

public class EmployeeDTO
{
    public Guid Id { get; set; }
    public string FullName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? HireDate { get; set; }
    public string? IdNumber { get; set; }
    public string? Gender { get; set; }
    public bool IsActive { get; set; }
    public string? ProfilePicture { get; set; }
    
    // Driver License (for driver positions)
    public DateOnly? DriverLicenseExpiryDate { get; set; }
    public Guid? DriverLicenseDocumentId { get; set; }
    
    // Foreign Keys
    public Guid PositionId { get; set; }
    public string? PositionName { get; set; }
    public bool? PositionIsDriverPosition { get; set; }
    public Guid? EmploymentTypeId { get; set; }
    public string? EmploymentTypeName { get; set; }
    public Guid UserId { get; set; }
    public string? Username { get; set; }
    public Guid RoleId { get; set; }
    public string? RoleName { get; set; }
    
    // Related Data
    public BiometricEnrolmentDTO? BiometricEnrolment { get; set; }
    public BankDetailsDTO? BankDetails { get; set; }
    public List<EmergencyContactDTO> EmergencyContacts { get; set; } = new();
    public List<string> CurrentTeams { get; set; } = new();
    
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateEmployeeDTO
{
    public string FullName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string Email { get; set; } = string.Empty; // Required for user account
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? HireDate { get; set; }
    public string? IdNumber { get; set; }
    public string? Gender { get; set; }
    public Guid PositionId { get; set; }
    public Guid? EmploymentTypeId { get; set; }
    public Guid RoleId { get; set; } // Required - system role
    public string? ProfilePicture { get; set; }
    
    // Driver License (required if position is driver position)
    public DateOnly? DriverLicenseExpiryDate { get; set; }
    public Guid? DriverLicenseDocumentId { get; set; }
}

public class UpdateEmployeeDTO
{
    public string FullName { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public string? IdNumber { get; set; }
    public string? Gender { get; set; }
    public Guid PositionId { get; set; }
    public Guid? EmploymentTypeId { get; set; }
    public Guid RoleId { get; set; }
    public bool IsActive { get; set; }
    public string? ProfilePicture { get; set; }
    
    // Driver License
    public DateOnly? DriverLicenseExpiryDate { get; set; }
    public Guid? DriverLicenseDocumentId { get; set; }
}

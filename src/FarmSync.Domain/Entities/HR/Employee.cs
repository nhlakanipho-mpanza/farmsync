using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class Employee : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string EmployeeNumber { get; set; } = string.Empty;
    public string? ContactNumber { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateOnly? DateOfBirth { get; set; }
    public DateOnly? HireDate { get; set; }
    public string? IdNumber { get; set; }
    public string? Gender { get; set; }
    public bool IsActive { get; set; } = true;
    public string? ProfilePicture { get; set; }

    // Foreign Keys
    public Guid PositionId { get; set; }
    public Guid? EmploymentTypeId { get; set; }
    public Guid? RoleTypeId { get; set; }

    // Navigation properties
    public virtual Position Position { get; set; } = null!;
    public virtual EmploymentType? EmploymentType { get; set; }
    public virtual RoleType? RoleType { get; set; }
    public virtual BiometricEnrolment? BiometricEnrolment { get; set; }
    public virtual BankDetails? BankDetails { get; set; }
    public virtual ICollection<EmergencyContact> EmergencyContacts { get; set; } = new List<EmergencyContact>();
    public virtual ICollection<TeamMember> TeamMemberships { get; set; } = new List<TeamMember>();
    public virtual ICollection<Team> TeamsLed { get; set; } = new List<Team>();
    public virtual ICollection<WorkTask> AssignedTasks { get; set; } = new List<WorkTask>();
    public virtual ICollection<ClockEvent> ClockEvents { get; set; } = new List<ClockEvent>();
}

using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class EmploymentType : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Seasonal, Permanent, Contractor
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

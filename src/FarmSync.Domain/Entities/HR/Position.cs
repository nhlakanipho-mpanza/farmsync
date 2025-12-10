using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class Position : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public decimal Rate { get; set; } // Hourly or daily rate
    public string? Description { get; set; }
    public bool IsDriverPosition { get; set; } = false; // Requires driver's license
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Employee> Employees { get; set; } = new List<Employee>();
}

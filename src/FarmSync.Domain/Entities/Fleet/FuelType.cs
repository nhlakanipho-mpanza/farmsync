using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.Fleet;

public class FuelType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation Properties
    public virtual ICollection<Vehicle> Vehicles { get; set; } = new List<Vehicle>();
}

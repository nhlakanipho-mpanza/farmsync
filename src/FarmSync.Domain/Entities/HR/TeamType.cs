using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.HR;

public class TeamType : BaseEntity
{
    public string Name { get; set; } = string.Empty; // Harvesting, Spraying, Packing, Irrigation, Maintenance
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;

    // Navigation properties
    public virtual ICollection<Team> Teams { get; set; } = new List<Team>();
}

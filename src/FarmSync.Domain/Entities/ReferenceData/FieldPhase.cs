using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.ReferenceData;

public class FieldPhase : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int SortOrder { get; set; } // Land Preparation(1) → Planting(2) → Growing(3) → Harvesting(4) → Haulage(5)
    public bool IsActive { get; set; } = true;
}

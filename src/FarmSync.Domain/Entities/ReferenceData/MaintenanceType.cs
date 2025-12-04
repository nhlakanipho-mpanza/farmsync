using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.ReferenceData;

public class MaintenanceType : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

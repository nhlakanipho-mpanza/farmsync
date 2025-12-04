using FarmSync.Domain.Common;

namespace FarmSync.Domain.Entities.ReferenceData;

public class EquipmentCondition : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public bool IsActive { get; set; } = true;
}

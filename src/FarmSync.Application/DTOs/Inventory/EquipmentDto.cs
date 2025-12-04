namespace FarmSync.Application.DTOs.Inventory;

public class EquipmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public string? Manufacturer { get; set; }
    public string ConditionName { get; set; } = string.Empty;
    public string? LocationName { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDue { get; set; }
    public bool IsActive { get; set; }
}

public class CreateEquipmentDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public string? Manufacturer { get; set; }
    public Guid ConditionId { get; set; }
    public Guid? LocationId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
}

public class UpdateEquipmentDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SerialNumber { get; set; }
    public string? Model { get; set; }
    public string? Manufacturer { get; set; }
    public Guid ConditionId { get; set; }
    public Guid? LocationId { get; set; }
    public DateTime? PurchaseDate { get; set; }
    public decimal? PurchasePrice { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDue { get; set; }
    public bool IsActive { get; set; }
}

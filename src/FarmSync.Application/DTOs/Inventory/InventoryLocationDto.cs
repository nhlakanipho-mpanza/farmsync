namespace FarmSync.Application.DTOs.Inventory;

public class InventoryLocationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}

public class CreateInventoryLocationDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
}

public class UpdateInventoryLocationDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? Address { get; set; }
    public bool IsActive { get; set; }
}

namespace FarmSync.Application.DTOs.Inventory;

public class InventoryItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string TypeName { get; set; } = string.Empty;
    public string UnitOfMeasureName { get; set; } = string.Empty;
    public decimal MinimumStockLevel { get; set; }
    public decimal ReorderLevel { get; set; }
    public decimal? UnitPrice { get; set; }
    public decimal AverageUnitCost { get; set; }
    public decimal TotalStock { get; set; }
    public decimal TotalStockValue { get; set; }  // TotalStock × AverageUnitCost
    public bool IsActive { get; set; }
}

public class CreateInventoryItemDto
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public Guid CategoryId { get; set; }
    public Guid TypeId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public decimal ReorderLevel { get; set; }
    public decimal? UnitPrice { get; set; }
}

public class UpdateInventoryItemDto
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string? SKU { get; set; }
    public Guid CategoryId { get; set; }
    public Guid TypeId { get; set; }
    public Guid UnitOfMeasureId { get; set; }
    public decimal MinimumStockLevel { get; set; }
    public decimal ReorderLevel { get; set; }
    public decimal? UnitPrice { get; set; }
    public bool IsActive { get; set; }
}

namespace FarmSync.Application.DTOs.Reports;

public class InventoryReportDto
{
    public string ItemName { get; set; } = string.Empty;
    public string Category { get; set; } = string.Empty;
    public string Sku { get; set; } = string.Empty;
    public decimal CurrentStock { get; set; }
    public decimal MinimumStock { get; set; }
    public decimal UnitPrice { get; set; }
    public decimal TotalValue { get; set; }
    public string Status { get; set; } = string.Empty; // In Stock, Low Stock, Out of Stock
}

public class InventoryValuationDto
{
    public decimal TotalInventoryValue { get; set; }
    public int TotalItems { get; set; }
    public int LowStockItems { get; set; }
    public int OutOfStockItems { get; set; }
    public List<InventoryReportDto> Items { get; set; } = new();
    public List<CategoryValuationDto> CategoryBreakdown { get; set; } = new();
}

public class CategoryValuationDto
{
    public string CategoryName { get; set; } = string.Empty;
    public int ItemCount { get; set; }
    public decimal TotalValue { get; set; }
    public decimal Percentage { get; set; }
}

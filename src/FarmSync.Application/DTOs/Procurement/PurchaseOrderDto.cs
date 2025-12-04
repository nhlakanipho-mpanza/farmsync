namespace FarmSync.Application.DTOs.Procurement;

public class PurchaseOrderDto
{
    public Guid Id { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public Guid SupplierId { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string? Notes { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public List<PurchaseOrderItemDto> Items { get; set; } = new();
}

public class PurchaseOrderItemDto
{
    public Guid Id { get; set; }
    public Guid PurchaseOrderId { get; set; }
    public Guid InventoryItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public string ItemSKU { get; set; } = string.Empty;
    public decimal OrderedQuantity { get; set; }
    public decimal ReceivedQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
    public string? Notes { get; set; }
}

public class CreatePurchaseOrderDto
{
    public Guid SupplierId { get; set; }
    public DateTime OrderDate { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? Notes { get; set; }
    public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
}

public class CreatePurchaseOrderItemDto
{
    public Guid InventoryItemId { get; set; }
    public decimal OrderedQuantity { get; set; }
    public decimal UnitPrice { get; set; }
    public string? Description { get; set; }
}

public class UpdatePurchaseOrderDto
{
    public Guid Id { get; set; }
    public Guid SupplierId { get; set; }
    public DateTime? ExpectedDeliveryDate { get; set; }
    public string? Notes { get; set; }
    public List<CreatePurchaseOrderItemDto> Items { get; set; } = new();
}

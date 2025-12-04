namespace FarmSync.Application.DTOs.Procurement;

public class GoodsReceivedDto
{
    public Guid Id { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public Guid PurchaseOrderId { get; set; }
    public string PONumber { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public Guid ReceivedBy { get; set; }
    public string Status { get; set; } = string.Empty;
    public bool HasDiscrepancies { get; set; }
    public string? DiscrepancyNotes { get; set; }
    public bool IsFinalReceipt { get; set; }
    public Guid? ApprovedBy { get; set; }
    public DateTime? ApprovedAt { get; set; }
    public List<GoodsReceivedItemDto> Items { get; set; } = new();
}

public class GoodsReceivedItemDto
{
    public Guid Id { get; set; }
    public Guid GoodsReceivedId { get; set; }
    public Guid PurchaseOrderItemId { get; set; }
    public string ItemName { get; set; } = string.Empty;
    public decimal OrderedQuantity { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal QuantityDamaged { get; set; }
    public decimal QuantityShortfall { get; set; }
    public string Condition { get; set; } = "Good";
    public string? Notes { get; set; }
}

public class CreateGoodsReceivedDto
{
    public Guid PurchaseOrderId { get; set; }
    public DateTime ReceivedDate { get; set; }
    public string? DiscrepancyNotes { get; set; }
    public bool? IsFinalReceipt { get; set; }
    public List<CreateGoodsReceivedItemDto> Items { get; set; } = new();
}

public class CreateGoodsReceivedItemDto
{
    public Guid PurchaseOrderItemId { get; set; }
    public decimal QuantityReceived { get; set; }
    public decimal QuantityDamaged { get; set; }
    public decimal QuantityShortfall { get; set; }
    public string Condition { get; set; } = "Good";
    public string? Notes { get; set; }
}

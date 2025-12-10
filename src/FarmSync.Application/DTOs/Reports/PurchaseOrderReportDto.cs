namespace FarmSync.Application.DTOs.Reports;

public class PurchaseOrderReportDto
{
    public string PoNumber { get; set; } = string.Empty;
    public DateTime OrderDate { get; set; }
    public string SupplierName { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
    public decimal TotalAmount { get; set; }
    public string RequestedBy { get; set; } = string.Empty;
    public DateTime? DeliveryDate { get; set; }
}

public class PurchaseOrderSummaryDto
{
    public int TotalOrders { get; set; }
    public decimal TotalAmount { get; set; }
    public int PendingOrders { get; set; }
    public int ApprovedOrders { get; set; }
    public int CompletedOrders { get; set; }
    public List<PurchaseOrderReportDto> Orders { get; set; } = new();
}

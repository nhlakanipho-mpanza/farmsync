namespace FarmSync.Application.DTOs.Reports;

public class GoodsReceivedReportDto
{
    public string GrnNumber { get; set; } = string.Empty;
    public DateTime ReceivedDate { get; set; }
    public string PoNumber { get; set; } = string.Empty;
    public string SupplierName { get; set; } = string.Empty;
    public int ItemsCount { get; set; }
    public decimal TotalValue { get; set; }
    public string ReceivedBy { get; set; } = string.Empty;
    public string Status { get; set; } = string.Empty;
}

public class GoodsReceivedSummaryDto
{
    public int TotalGrns { get; set; }
    public decimal TotalValue { get; set; }
    public int PendingInspections { get; set; }
    public int Approved { get; set; }
    public List<GoodsReceivedReportDto> GoodsReceived { get; set; } = new();
}

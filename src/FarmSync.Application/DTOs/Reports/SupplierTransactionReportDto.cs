namespace FarmSync.Application.DTOs.Reports;

public class SupplierTransactionReportDto
{
    public string SupplierName { get; set; } = string.Empty;
    public DateTime TransactionDate { get; set; }
    public string TransactionType { get; set; } = string.Empty; // Purchase, Payment, Credit Note
    public string ReferenceNumber { get; set; } = string.Empty;
    public decimal Amount { get; set; }
    public decimal Balance { get; set; }
    public string Description { get; set; } = string.Empty;
}

public class SupplierTransactionSummaryDto
{
    public string SupplierName { get; set; } = string.Empty;
    public decimal TotalPurchases { get; set; }
    public decimal TotalPayments { get; set; }
    public decimal CurrentBalance { get; set; }
    public int TransactionCount { get; set; }
    public List<SupplierTransactionReportDto> Transactions { get; set; } = new();
}

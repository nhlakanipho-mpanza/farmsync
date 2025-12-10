using FarmSync.Application.DTOs.Reports;

namespace FarmSync.Application.Interfaces;

public interface IReportService
{
    Task<PurchaseOrderSummaryDto> GetPurchaseOrderReportAsync(ReportFilterDto filter);
    Task<GoodsReceivedSummaryDto> GetGoodsReceivedReportAsync(ReportFilterDto filter);
    Task<InventoryValuationDto> GetInventoryValuationReportAsync(ReportFilterDto filter);
    Task<SupplierTransactionSummaryDto> GetSupplierTransactionReportAsync(ReportFilterDto filter);
    Task<ExpenseSummaryDto> GetExpenseReportAsync(ReportFilterDto filter);
}

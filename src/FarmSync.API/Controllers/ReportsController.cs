using FarmSync.Application.DTOs.Reports;
using FarmSync.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace FarmSync.API.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ReportsController : ControllerBase
{
    private readonly IReportService _reportService;
    private readonly IExportService _exportService;
    private readonly ILogger<ReportsController> _logger;

    public ReportsController(
        IReportService reportService,
        IExportService exportService,
        ILogger<ReportsController> logger)
    {
        _reportService = reportService;
        _exportService = exportService;
        _logger = logger;
    }

    [HttpPost("purchase-orders")]
    public async Task<ActionResult<PurchaseOrderSummaryDto>> GetPurchaseOrderReport([FromBody] ReportFilterDto filter)
    {
        try
        {
            var report = await _reportService.GetPurchaseOrderReportAsync(filter);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating purchase order report");
            return StatusCode(500, "Error generating report");
        }
    }

    [HttpPost("purchase-orders/export")]
    public async Task<IActionResult> ExportPurchaseOrderReport([FromBody] ReportFilterDto filter, [FromQuery] ExportFormat format = ExportFormat.Pdf)
    {
        try
        {
            var report = await _reportService.GetPurchaseOrderReportAsync(filter);
            
            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == ExportFormat.Pdf)
            {
                fileBytes = await _exportService.ExportToPdfAsync(report, "Purchase Orders Report");
                contentType = "application/pdf";
                fileName = $"PurchaseOrders_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            }
            else
            {
                fileBytes = await _exportService.ExportToExcelAsync(report.Orders, "Purchase Orders");
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"PurchaseOrders_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            }

            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting purchase order report");
            return StatusCode(500, "Error exporting report");
        }
    }

    [HttpPost("goods-received")]
    public async Task<ActionResult<GoodsReceivedSummaryDto>> GetGoodsReceivedReport([FromBody] ReportFilterDto filter)
    {
        try
        {
            var report = await _reportService.GetGoodsReceivedReportAsync(filter);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating goods received report");
            return StatusCode(500, "Error generating report");
        }
    }

    [HttpPost("goods-received/export")]
    public async Task<IActionResult> ExportGoodsReceivedReport([FromBody] ReportFilterDto filter, [FromQuery] ExportFormat format = ExportFormat.Pdf)
    {
        try
        {
            var report = await _reportService.GetGoodsReceivedReportAsync(filter);
            
            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == ExportFormat.Pdf)
            {
                fileBytes = await _exportService.ExportToPdfAsync(report, "Goods Received Report");
                contentType = "application/pdf";
                fileName = $"GoodsReceived_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            }
            else
            {
                fileBytes = await _exportService.ExportToExcelAsync(report.GoodsReceived, "Goods Received");
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"GoodsReceived_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            }

            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting goods received report");
            return StatusCode(500, "Error exporting report");
        }
    }

    [HttpPost("inventory-valuation")]
    public async Task<ActionResult<InventoryValuationDto>> GetInventoryValuationReport([FromBody] ReportFilterDto filter)
    {
        try
        {
            var report = await _reportService.GetInventoryValuationReportAsync(filter);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating inventory valuation report");
            return StatusCode(500, "Error generating report");
        }
    }

    [HttpPost("inventory-valuation/export")]
    public async Task<IActionResult> ExportInventoryValuationReport([FromBody] ReportFilterDto filter, [FromQuery] ExportFormat format = ExportFormat.Pdf)
    {
        try
        {
            var report = await _reportService.GetInventoryValuationReportAsync(filter);
            
            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == ExportFormat.Pdf)
            {
                fileBytes = await _exportService.ExportToPdfAsync(report, "Inventory Valuation Report");
                contentType = "application/pdf";
                fileName = $"InventoryValuation_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            }
            else
            {
                fileBytes = await _exportService.ExportToExcelAsync(report.Items, "Inventory");
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"InventoryValuation_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            }

            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting inventory valuation report");
            return StatusCode(500, "Error exporting report");
        }
    }

    [HttpPost("supplier-transactions")]
    public async Task<ActionResult<SupplierTransactionSummaryDto>> GetSupplierTransactionReport([FromBody] ReportFilterDto filter)
    {
        try
        {
            var report = await _reportService.GetSupplierTransactionReportAsync(filter);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating supplier transaction report");
            return StatusCode(500, ex.Message);
        }
    }

    [HttpPost("supplier-transactions/export")]
    public async Task<IActionResult> ExportSupplierTransactionReport([FromBody] ReportFilterDto filter, [FromQuery] ExportFormat format = ExportFormat.Pdf)
    {
        try
        {
            var report = await _reportService.GetSupplierTransactionReportAsync(filter);
            
            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == ExportFormat.Pdf)
            {
                fileBytes = await _exportService.ExportToPdfAsync(report, $"Supplier Transactions - {report.SupplierName}");
                contentType = "application/pdf";
                fileName = $"SupplierTransactions_{report.SupplierName}_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            }
            else
            {
                fileBytes = await _exportService.ExportToExcelAsync(report.Transactions, "Transactions");
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"SupplierTransactions_{report.SupplierName}_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            }

            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting supplier transaction report");
            return StatusCode(500, "Error exporting report");
        }
    }

    [HttpPost("expenses")]
    public async Task<ActionResult<ExpenseSummaryDto>> GetExpenseReport([FromBody] ReportFilterDto filter)
    {
        try
        {
            var report = await _reportService.GetExpenseReportAsync(filter);
            return Ok(report);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error generating expense report");
            return StatusCode(500, "Error generating report");
        }
    }

    [HttpPost("expenses/export")]
    public async Task<IActionResult> ExportExpenseReport([FromBody] ReportFilterDto filter, [FromQuery] ExportFormat format = ExportFormat.Pdf)
    {
        try
        {
            var report = await _reportService.GetExpenseReportAsync(filter);
            
            byte[] fileBytes;
            string contentType;
            string fileName;

            if (format == ExportFormat.Pdf)
            {
                fileBytes = await _exportService.ExportToPdfAsync(report, "Expense Report");
                contentType = "application/pdf";
                fileName = $"Expenses_{DateTime.Now:yyyyMMdd_HHmmss}.pdf";
            }
            else
            {
                fileBytes = await _exportService.ExportToExcelAsync(report.Expenses, "Expenses");
                contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                fileName = $"Expenses_{DateTime.Now:yyyyMMdd_HHmmss}.xlsx";
            }

            return File(fileBytes, contentType, fileName);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error exporting expense report");
            return StatusCode(500, "Error exporting report");
        }
    }
}

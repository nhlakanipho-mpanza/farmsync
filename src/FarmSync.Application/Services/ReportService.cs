using FarmSync.Application.DTOs.Reports;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Application.Services;

public class ReportService : IReportService
{
    private readonly IPurchaseOrderRepository _purchaseOrderRepository;
    private readonly IGoodsReceivedRepository _goodsReceivedRepository;
    private readonly IRepository<FarmSync.Domain.Entities.Inventory.InventoryItem> _inventoryRepository;
    private readonly ISupplierRepository _supplierRepository;

    public ReportService(
        IPurchaseOrderRepository purchaseOrderRepository,
        IGoodsReceivedRepository goodsReceivedRepository,
        IRepository<FarmSync.Domain.Entities.Inventory.InventoryItem> inventoryRepository,
        ISupplierRepository supplierRepository)
    {
        _purchaseOrderRepository = purchaseOrderRepository;
        _goodsReceivedRepository = goodsReceivedRepository;
        _inventoryRepository = inventoryRepository;
        _supplierRepository = supplierRepository;
    }

    public async Task<PurchaseOrderSummaryDto> GetPurchaseOrderReportAsync(ReportFilterDto filter)
    {
        var allOrders = await _purchaseOrderRepository.GetAllAsync();
        var query = allOrders.AsQueryable();

        // Apply filters
        if (filter.StartDate.HasValue)
            query = query.Where(po => po.OrderDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(po => po.OrderDate <= filter.EndDate.Value);

        if (filter.SupplierId.HasValue)
            query = query.Where(po => po.SupplierId == filter.SupplierId.Value);

        var purchaseOrders = query.ToList();

        var summary = new PurchaseOrderSummaryDto
        {
            TotalOrders = purchaseOrders.Count,
            TotalAmount = purchaseOrders.Sum(po => po.TotalAmount),
            PendingOrders = purchaseOrders.Count(po => po.Status == Domain.Entities.Procurement.POStatus.Created),
            ApprovedOrders = purchaseOrders.Count(po => po.Status == Domain.Entities.Procurement.POStatus.Approved),
            CompletedOrders = purchaseOrders.Count(po => po.Status == Domain.Entities.Procurement.POStatus.FullyReceived),
            Orders = purchaseOrders.Select(po => new PurchaseOrderReportDto
            {
                PoNumber = po.PONumber,
                OrderDate = po.OrderDate,
                SupplierName = po.Supplier?.Name ?? "Unknown",
                Status = po.Status.ToString(),
                TotalAmount = po.TotalAmount,
                RequestedBy = "Unknown", // Not in entity
                DeliveryDate = po.ExpectedDeliveryDate
            }).ToList()
        };

        return summary;
    }

    public async Task<GoodsReceivedSummaryDto> GetGoodsReceivedReportAsync(ReportFilterDto filter)
    {
        var allGrns = await _goodsReceivedRepository.GetAllAsync();
        var query = allGrns.AsQueryable();

        // Apply filters
        if (filter.StartDate.HasValue)
            query = query.Where(grn => grn.ReceivedDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(grn => grn.ReceivedDate <= filter.EndDate.Value);

        if (filter.SupplierId.HasValue)
            query = query.Where(grn => grn.PurchaseOrder != null && grn.PurchaseOrder.SupplierId == filter.SupplierId.Value);

        var goodsReceived = query.ToList();

        var summary = new GoodsReceivedSummaryDto
        {
            TotalGrns = goodsReceived.Count,
            TotalValue = goodsReceived.Sum(grn => grn.PurchaseOrder?.TotalAmount ?? 0),
            PendingInspections = goodsReceived.Count(grn => grn.Status == Domain.Entities.Procurement.GRStatus.Pending),
            Approved = goodsReceived.Count(grn => grn.Status == Domain.Entities.Procurement.GRStatus.Approved),
            GoodsReceived = goodsReceived.Select(grn => new GoodsReceivedReportDto
            {
                GrnNumber = grn.ReceiptNumber,
                ReceivedDate = grn.ReceivedDate,
                PoNumber = grn.PurchaseOrder?.PONumber ?? "N/A",
                SupplierName = grn.PurchaseOrder?.Supplier?.Name ?? "Unknown",
                ItemsCount = grn.Items?.Count ?? 0,
                TotalValue = grn.PurchaseOrder?.TotalAmount ?? 0,
                ReceivedBy = "Unknown", // ReceivedBy is Guid, need to lookup user
                Status = grn.Status.ToString()
            }).ToList()
        };

        return summary;
    }

    public async Task<InventoryValuationDto> GetInventoryValuationReportAsync(ReportFilterDto filter)
    {
        var allItems = await _inventoryRepository.GetAllAsync();
        var query = allItems.AsQueryable();

        // Apply filters
        if (filter.CategoryId.HasValue)
            query = query.Where(item => item.CategoryId == filter.CategoryId.Value);

        var inventoryItems = query.ToList();

        var totalValue = inventoryItems.Sum(item => item.CurrentStockLevel * (item.UnitPrice ?? 0));

        var categoryBreakdown = inventoryItems
            .GroupBy(item => item.Category?.Name ?? "Uncategorized")
            .Select(g => new CategoryValuationDto
            {
                CategoryName = g.Key,
                ItemCount = g.Count(),
                TotalValue = g.Sum(item => item.CurrentStockLevel * (item.UnitPrice ?? 0)),
                Percentage = totalValue > 0 ? (g.Sum(item => item.CurrentStockLevel * (item.UnitPrice ?? 0)) / totalValue) * 100 : 0
            })
            .ToList();

        var valuation = new InventoryValuationDto
        {
            TotalInventoryValue = totalValue,
            TotalItems = inventoryItems.Count,
            LowStockItems = inventoryItems.Count(item => item.CurrentStockLevel <= item.MinimumStockLevel && item.CurrentStockLevel > 0),
            OutOfStockItems = inventoryItems.Count(item => item.CurrentStockLevel <= 0),
            CategoryBreakdown = categoryBreakdown,
            Items = inventoryItems.Select(item => new InventoryReportDto
            {
                ItemName = item.Name,
                Category = item.Category?.Name ?? "Uncategorized",
                Sku = item.SKU ?? "",
                CurrentStock = item.CurrentStockLevel,
                MinimumStock = item.MinimumStockLevel,
                UnitPrice = item.UnitPrice ?? 0,
                TotalValue = item.CurrentStockLevel * (item.UnitPrice ?? 0),
                Status = item.CurrentStockLevel <= 0 ? "Out of Stock" :
                         item.CurrentStockLevel <= item.MinimumStockLevel ? "Low Stock" : "In Stock"
            }).ToList()
        };

        return valuation;
    }

    public async Task<SupplierTransactionSummaryDto> GetSupplierTransactionReportAsync(ReportFilterDto filter)
    {
        if (!filter.SupplierId.HasValue)
            throw new ArgumentException("SupplierId is required for supplier transaction report");

        var supplier = await _supplierRepository.GetByIdAsync(filter.SupplierId.Value);
        if (supplier == null)
            throw new KeyNotFoundException("Supplier not found");

        // Get purchase orders for the supplier
        var allOrders = await _purchaseOrderRepository.GetAllAsync();
        var purchaseOrders = allOrders.Where(po => po.SupplierId == filter.SupplierId.Value);

        if (filter.StartDate.HasValue)
            purchaseOrders = purchaseOrders.Where(po => po.OrderDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            purchaseOrders = purchaseOrders.Where(po => po.OrderDate <= filter.EndDate.Value);

        var transactions = new List<SupplierTransactionReportDto>();
        decimal runningBalance = 0;

        foreach (var po in purchaseOrders.OrderBy(po => po.OrderDate))
        {
            runningBalance += po.TotalAmount;
            transactions.Add(new SupplierTransactionReportDto
            {
                SupplierName = supplier.Name,
                TransactionDate = po.OrderDate,
                TransactionType = "Purchase",
                ReferenceNumber = po.PONumber,
                Amount = po.TotalAmount,
                Balance = runningBalance,
                Description = $"Purchase Order {po.PONumber}"
            });
        }

        var summary = new SupplierTransactionSummaryDto
        {
            SupplierName = supplier.Name,
            TotalPurchases = purchaseOrders.Sum(po => po.TotalAmount),
            TotalPayments = 0, // TODO: Implement payments tracking
            CurrentBalance = runningBalance,
            TransactionCount = transactions.Count,
            Transactions = transactions
        };

        return summary;
    }

    public async Task<ExpenseSummaryDto> GetExpenseReportAsync(ReportFilterDto filter)
    {
        var allOrders = await _purchaseOrderRepository.GetAllAsync();
        var query = allOrders.AsQueryable();

        // Apply filters
        if (filter.StartDate.HasValue)
            query = query.Where(po => po.OrderDate >= filter.StartDate.Value);

        if (filter.EndDate.HasValue)
            query = query.Where(po => po.OrderDate <= filter.EndDate.Value);

        var expenses = query.ToList();
        var totalExpenses = expenses.Sum(e => e.TotalAmount);

        // Group by category (using supplier as category for now)
        var categoryBreakdown = expenses
            .GroupBy(e => e.Supplier?.Name ?? "Uncategorized")
            .Select(g => new CategoryExpenseDto
            {
                CategoryName = g.Key,
                TotalAmount = g.Sum(e => e.TotalAmount),
                TransactionCount = g.Count(),
                Percentage = totalExpenses > 0 ? (g.Sum(e => e.TotalAmount) / totalExpenses) * 100 : 0
            })
            .ToList();

        var summary = new ExpenseSummaryDto
        {
            TotalExpenses = totalExpenses,
            StartDate = filter.StartDate ?? DateTime.MinValue,
            EndDate = filter.EndDate ?? DateTime.MaxValue,
            CategoryBreakdown = categoryBreakdown,
            DepartmentBreakdown = new List<DepartmentExpenseDto>(), // Not available in PurchaseOrder entity
            Expenses = expenses.Select(e => new ExpenseReportDto
            {
                ExpenseDate = e.OrderDate,
                Category = e.Supplier?.Name ?? "Uncategorized",
                Description = $"PO {e.PONumber}",
                Amount = e.TotalAmount,
                Department = "N/A", // Not available in PurchaseOrder entity
                RequestedBy = "Unknown", // Not available in PurchaseOrder entity
                Status = e.Status.ToString()
            }).ToList()
        };

        return summary;
    }
}

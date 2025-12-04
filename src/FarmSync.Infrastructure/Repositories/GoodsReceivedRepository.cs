using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Interfaces;
using FarmSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FarmSync.Infrastructure.Repositories;

public class GoodsReceivedRepository : Repository<GoodsReceived>, IGoodsReceivedRepository
{
    public GoodsReceivedRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<GoodsReceived>> GetAllWithDetailsAsync()
    {
        return await _context.GoodsReceived
            .Include(gr => gr.PurchaseOrder)
                .ThenInclude(po => po.Supplier)
            .Include(gr => gr.Items)
                .ThenInclude(gri => gri.PurchaseOrderItem)
                    .ThenInclude(poi => poi.InventoryItem)
            .OrderByDescending(gr => gr.ReceivedDate)
            .ToListAsync();
    }

    public async Task<GoodsReceived?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.GoodsReceived
            .Include(gr => gr.PurchaseOrder)
                .ThenInclude(po => po.Supplier)
            .Include(gr => gr.Items)
                .ThenInclude(gri => gri.PurchaseOrderItem)
                    .ThenInclude(poi => poi.InventoryItem)
            .FirstOrDefaultAsync(gr => gr.Id == id);
    }

    public async Task<IEnumerable<GoodsReceived>> GetByPurchaseOrderAsync(Guid purchaseOrderId)
    {
        return await _context.GoodsReceived
            .Include(gr => gr.Items)
                .ThenInclude(gri => gri.PurchaseOrderItem)
                    .ThenInclude(poi => poi.InventoryItem)
            .Where(gr => gr.PurchaseOrderId == purchaseOrderId)
            .OrderByDescending(gr => gr.ReceivedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<GoodsReceived>> GetPendingApprovalsAsync()
    {
        return await _context.GoodsReceived
            .Include(gr => gr.PurchaseOrder)
                .ThenInclude(po => po.Supplier)
            .Include(gr => gr.Items)
                .ThenInclude(gri => gri.PurchaseOrderItem)
                    .ThenInclude(poi => poi.InventoryItem)
            .Where(gr => gr.HasDiscrepancies && gr.Status == GRStatus.Pending)
            .OrderBy(gr => gr.ReceivedDate)
            .ToListAsync();
    }

    public async Task<string> GenerateReceiptNumberAsync()
    {
        var year = DateTime.Now.Year;
        var prefix = $"GR-{year}-";
        
        var lastReceipt = await _context.GoodsReceived
            .Where(gr => gr.ReceiptNumber.StartsWith(prefix))
            .OrderByDescending(gr => gr.ReceiptNumber)
            .FirstOrDefaultAsync();

        if (lastReceipt == null)
        {
            return $"{prefix}0001";
        }

        var lastNumber = int.Parse(lastReceipt.ReceiptNumber.Replace(prefix, ""));
        var newNumber = lastNumber + 1;
        
        return $"{prefix}{newNumber:D4}";
    }
}

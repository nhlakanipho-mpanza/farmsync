using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Interfaces;
using FarmSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FarmSync.Infrastructure.Repositories;

public class PurchaseOrderRepository : Repository<PurchaseOrder>, IPurchaseOrderRepository
{
    public PurchaseOrderRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<PurchaseOrder>> GetAllWithDetailsAsync()
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Items)
                .ThenInclude(item => item.InventoryItem)
            .Include(po => po.GoodsReceivedRecords)
            .OrderByDescending(po => po.CreatedAt)
            .ToListAsync();
    }

    public async Task<PurchaseOrder?> GetByIdWithDetailsAsync(Guid id)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Items)
                .ThenInclude(item => item.InventoryItem)
            .Include(po => po.GoodsReceivedRecords)
                .ThenInclude(gr => gr.Items)
            .FirstOrDefaultAsync(po => po.Id == id);
    }

    public async Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(POStatus status)
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Items)
                .ThenInclude(item => item.InventoryItem)
            .Where(po => po.Status == status)
            .OrderByDescending(po => po.CreatedAt)
            .ToListAsync();
    }

    public async Task<IEnumerable<PurchaseOrder>> GetPendingApprovalsAsync()
    {
        return await _context.PurchaseOrders
            .Include(po => po.Supplier)
            .Include(po => po.Items)
                .ThenInclude(item => item.InventoryItem)
            .Where(po => po.Status == POStatus.Created)
            .OrderBy(po => po.OrderDate)
            .ToListAsync();
    }

    public async Task<string> GeneratePONumberAsync()
    {
        var year = DateTime.Now.Year;
        var prefix = $"PO-{year}-";
        
        var lastPO = await _context.PurchaseOrders
            .Where(po => po.PONumber.StartsWith(prefix))
            .OrderByDescending(po => po.PONumber)
            .FirstOrDefaultAsync();

        if (lastPO == null)
        {
            return $"{prefix}0001";
        }

        var lastNumber = int.Parse(lastPO.PONumber.Replace(prefix, ""));
        var newNumber = lastNumber + 1;
        
        return $"{prefix}{newNumber:D4}";
    }
}

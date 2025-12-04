using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories;

public class InventoryItemRepository : Repository<InventoryItem>
{
    public InventoryItemRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public override async Task<InventoryItem?> GetByIdAsync(Guid id)
    {
        return await _dbSet
            .Include(i => i.Category)
            .Include(i => i.Type)
            .Include(i => i.UnitOfMeasure)
            .Include(i => i.StockLevels)
                .ThenInclude(s => s.Location)
            .FirstOrDefaultAsync(i => i.Id == id);
    }

    public override async Task<IEnumerable<InventoryItem>> GetAllAsync()
    {
        return await _dbSet
            .Include(i => i.Category)
            .Include(i => i.Type)
            .Include(i => i.UnitOfMeasure)
            .Include(i => i.StockLevels)
            .Where(i => i.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync()
    {
        return await _dbSet
            .Include(i => i.Category)
            .Include(i => i.Type)
            .Include(i => i.UnitOfMeasure)
            .Include(i => i.StockLevels)
            .Where(i => i.IsActive && 
                   i.StockLevels.Sum(s => s.Quantity) <= i.ReorderLevel)
            .ToListAsync();
    }
}

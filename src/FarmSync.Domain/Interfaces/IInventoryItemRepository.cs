using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Application.Interfaces;

public interface IInventoryItemRepository : IRepository<InventoryItem>
{
    Task<IEnumerable<InventoryItem>> GetLowStockItemsAsync();
}

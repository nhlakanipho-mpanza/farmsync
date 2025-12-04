using FarmSync.Application.DTOs.Inventory;

namespace FarmSync.Application.Interfaces;

public interface IInventoryItemService
{
    Task<IEnumerable<InventoryItemDto>> GetAllAsync();
    Task<InventoryItemDto?> GetByIdAsync(Guid id);
    Task<InventoryItemDto> CreateAsync(CreateInventoryItemDto dto);
    Task<InventoryItemDto> UpdateAsync(UpdateInventoryItemDto dto);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<InventoryItemDto>> GetLowStockItemsAsync();
}

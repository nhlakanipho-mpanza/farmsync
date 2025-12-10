using FarmSync.Domain.Entities.Inventory;

namespace FarmSync.Domain.Interfaces;

public interface IEquipmentRepository
{
    Task<IEnumerable<Equipment>> GetAllAsync();
    Task<Equipment?> GetByIdAsync(Guid id);
    Task<IEnumerable<Equipment>> GetActiveAsync();
    Task<IEnumerable<Equipment>> GetByConditionAsync(Guid conditionId);
    Task<IEnumerable<Equipment>> GetByLocationAsync(Guid locationId);
    Task AddAsync(Equipment equipment);
    Task UpdateAsync(Equipment equipment);
    Task DeleteAsync(Guid id);
}

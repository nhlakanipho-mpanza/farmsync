using FarmSync.Application.DTOs.Inventory;

namespace FarmSync.Application.Interfaces;

public interface IEquipmentService
{
    Task<IEnumerable<EquipmentDto>> GetAllAsync();
    Task<EquipmentDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<EquipmentDto>> GetActiveAsync();
    Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto);
    Task<EquipmentDto> UpdateAsync(Guid id, UpdateEquipmentDto dto);
    Task DeleteAsync(Guid id);
}

using FarmSync.Application.DTOs.Procurement;

namespace FarmSync.Application.Interfaces;

public interface ISupplierService
{
    Task<IEnumerable<SupplierDto>> GetAllAsync();
    Task<IEnumerable<SupplierDto>> GetActiveAsync();
    Task<SupplierDto?> GetByIdAsync(Guid id);
    Task<SupplierDto> CreateAsync(CreateSupplierDto dto, Guid createdBy);
    Task<SupplierDto> UpdateAsync(Guid id, CreateSupplierDto dto, Guid updatedBy);
    Task DeleteAsync(Guid id);
}

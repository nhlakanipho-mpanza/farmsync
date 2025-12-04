using FarmSync.Application.DTOs.Procurement;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Application.Services;

public class SupplierService : ISupplierService
{
    private readonly ISupplierRepository _repository;
    private readonly IUnitOfWork _unitOfWork;

    public SupplierService(ISupplierRepository repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<SupplierDto>> GetAllAsync()
    {
        var suppliers = await _repository.GetAllAsync();
        return suppliers.Select(MapToDto);
    }

    public async Task<IEnumerable<SupplierDto>> GetActiveAsync()
    {
        var suppliers = await _repository.GetActiveAsync();
        return suppliers.Select(MapToDto);
    }

    public async Task<SupplierDto?> GetByIdAsync(Guid id)
    {
        var supplier = await _repository.GetByIdAsync(id);
        return supplier == null ? null : MapToDto(supplier);
    }

    public async Task<SupplierDto> CreateAsync(CreateSupplierDto dto, Guid createdBy)
    {
        // Check for duplicate name
        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing != null)
        {
            throw new InvalidOperationException($"A supplier with the name '{dto.Name}' already exists.");
        }

        var supplier = new Supplier
        {
            Name = dto.Name,
            ContactPerson = dto.ContactPerson,
            Email = dto.Email,
            Phone = dto.Phone,
            Address = dto.Address,
            TaxNumber = dto.TaxNumber,
            IsActive = true,
            CreatedBy = createdBy.ToString(),
            CreatedAt = DateTime.UtcNow
        };

        await _repository.AddAsync(supplier);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(supplier);
    }

    public async Task<SupplierDto> UpdateAsync(Guid id, CreateSupplierDto dto, Guid updatedBy)
    {
        var supplier = await _repository.GetByIdAsync(id);
        if (supplier == null)
        {
            throw new KeyNotFoundException($"Supplier with ID {id} not found.");
        }

        // Check for duplicate name (excluding current supplier)
        var existing = await _repository.GetByNameAsync(dto.Name);
        if (existing != null && existing.Id != id)
        {
            throw new InvalidOperationException($"A supplier with the name '{dto.Name}' already exists.");
        }

        supplier.Name = dto.Name;
        supplier.ContactPerson = dto.ContactPerson;
        supplier.Email = dto.Email;
        supplier.Phone = dto.Phone;
        supplier.Address = dto.Address;
        supplier.TaxNumber = dto.TaxNumber;
        supplier.UpdatedBy = updatedBy.ToString();
        supplier.UpdatedAt = DateTime.UtcNow;

        await _repository.UpdateAsync(supplier);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(supplier);
    }

    public async Task DeleteAsync(Guid id)
    {
        var supplier = await _repository.GetByIdAsync(id);
        if (supplier == null)
        {
            throw new KeyNotFoundException($"Supplier with ID {id} not found.");
        }

        // Check if supplier has any purchase orders
        if (supplier.PurchaseOrders.Any())
        {
            throw new InvalidOperationException("Cannot delete supplier with existing purchase orders. Set as inactive instead.");
        }

        await _repository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    private static SupplierDto MapToDto(Supplier supplier)
    {
        return new SupplierDto
        {
            Id = supplier.Id,
            Name = supplier.Name,
            ContactPerson = supplier.ContactPerson,
            Email = supplier.Email,
            Phone = supplier.Phone,
            Address = supplier.Address,
            TaxNumber = supplier.TaxNumber,
            IsActive = supplier.IsActive
        };
    }
}

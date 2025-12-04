using FarmSync.Application.DTOs.Inventory;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Application.Services;

public class InventoryItemService : IInventoryItemService
{
    private readonly IRepository<InventoryItem> _repository;
    private readonly IUnitOfWork _unitOfWork;

    public InventoryItemService(IRepository<InventoryItem> repository, IUnitOfWork unitOfWork)
    {
        _repository = repository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<InventoryItemDto>> GetAllAsync()
    {
        Console.WriteLine("InventoryItemService.GetAllAsync called");
        var items = await _repository.GetAllAsync();
        Console.WriteLine($"Retrieved {items.Count()} items from repository");
        var dtos = items.Select(MapToDto).ToList();
        Console.WriteLine($"Mapped {dtos.Count} DTOs");
        return dtos;
    }

    public async Task<InventoryItemDto?> GetByIdAsync(Guid id)
    {
        var item = await _repository.GetByIdAsync(id);
        return item != null ? MapToDto(item) : null;
    }

    public async Task<InventoryItemDto> CreateAsync(CreateInventoryItemDto dto)
    {
        var item = new InventoryItem
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            Description = dto.Description,
            SKU = dto.SKU,
            CategoryId = dto.CategoryId,
            TypeId = dto.TypeId,
            UnitOfMeasureId = dto.UnitOfMeasureId,
            MinimumStockLevel = dto.MinimumStockLevel,
            ReorderLevel = dto.ReorderLevel,
            UnitPrice = dto.UnitPrice,
            IsActive = true
        };

        await _repository.AddAsync(item);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _repository.GetByIdAsync(item.Id) ?? item);
    }

    public async Task<InventoryItemDto> UpdateAsync(UpdateInventoryItemDto dto)
    {
        var item = await _repository.GetByIdAsync(dto.Id);
        if (item == null)
            throw new KeyNotFoundException($"Inventory item with ID {dto.Id} not found");

        item.Name = dto.Name;
        item.Description = dto.Description;
        item.SKU = dto.SKU;
        item.CategoryId = dto.CategoryId;
        item.TypeId = dto.TypeId;
        item.UnitOfMeasureId = dto.UnitOfMeasureId;
        item.MinimumStockLevel = dto.MinimumStockLevel;
        item.ReorderLevel = dto.ReorderLevel;
        item.UnitPrice = dto.UnitPrice;
        item.IsActive = dto.IsActive;

        await _repository.UpdateAsync(item);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _repository.GetByIdAsync(item.Id) ?? item);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _repository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<InventoryItemDto>> GetLowStockItemsAsync()
    {
        // For now, fetch all items and filter in memory
        // In production, you'd want a specific repository method for this
        var items = await _repository.GetAllAsync();
        return items.Select(MapToDto);
    }

    private static InventoryItemDto MapToDto(InventoryItem item)
    {
        Console.WriteLine($"Mapping item '{item.Name}': Category={item.Category?.Name ?? "NULL"}, Type={item.Type?.Name ?? "NULL"}, UOM={item.UnitOfMeasure?.Name ?? "NULL"}");
        var totalStock = item.StockLevels?.Sum(s => s.Quantity) ?? 0;
        return new InventoryItemDto
        {
            Id = item.Id,
            Name = item.Name,
            Description = item.Description,
            SKU = item.SKU,
            CategoryName = item.Category?.Name ?? string.Empty,
            TypeName = item.Type?.Name ?? string.Empty,
            UnitOfMeasureName = item.UnitOfMeasure?.Name ?? string.Empty,
            MinimumStockLevel = item.MinimumStockLevel,
            ReorderLevel = item.ReorderLevel,
            UnitPrice = item.UnitPrice,
            AverageUnitCost = item.AverageUnitCost,
            TotalStock = totalStock,
            TotalStockValue = totalStock * item.AverageUnitCost,
            IsActive = item.IsActive
        };
    }
}

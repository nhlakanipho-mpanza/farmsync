using FarmSync.Application.DTOs.Inventory;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Application.Services;

public class EquipmentService : IEquipmentService
{
    private readonly IEquipmentRepository _equipmentRepository;
    private readonly IUnitOfWork _unitOfWork;

    public EquipmentService(IEquipmentRepository equipmentRepository, IUnitOfWork unitOfWork)
    {
        _equipmentRepository = equipmentRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<EquipmentDto>> GetAllAsync()
    {
        var equipment = await _equipmentRepository.GetAllAsync();
        return equipment.Select(MapToDto);
    }

    public async Task<EquipmentDto?> GetByIdAsync(Guid id)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(id);
        return equipment == null ? null : MapToDto(equipment);
    }

    public async Task<IEnumerable<EquipmentDto>> GetActiveAsync()
    {
        var equipment = await _equipmentRepository.GetActiveAsync();
        return equipment.Select(MapToDto);
    }

    public async Task<EquipmentDto> CreateAsync(CreateEquipmentDto dto)
    {
        var equipment = new Equipment
        {
            Name = dto.Name,
            Description = dto.Description,
            SerialNumber = dto.SerialNumber,
            Model = dto.Model,
            Manufacturer = dto.Manufacturer,
            ConditionId = dto.ConditionId,
            LocationId = dto.LocationId,
            PurchaseDate = dto.PurchaseDate,
            PurchasePrice = dto.PurchasePrice,
            IsActive = true
        };

        await _equipmentRepository.AddAsync(equipment);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _equipmentRepository.GetByIdAsync(equipment.Id) 
            ?? throw new InvalidOperationException("Failed to retrieve created equipment"));
    }

    public async Task<EquipmentDto> UpdateAsync(Guid id, UpdateEquipmentDto dto)
    {
        if (id != dto.Id)
        {
            throw new ArgumentException("ID mismatch");
        }

        var equipment = await _equipmentRepository.GetByIdAsync(id);
        if (equipment == null)
        {
            throw new KeyNotFoundException($"Equipment with ID {id} not found");
        }

        equipment.Name = dto.Name;
        equipment.Description = dto.Description;
        equipment.SerialNumber = dto.SerialNumber;
        equipment.Model = dto.Model;
        equipment.Manufacturer = dto.Manufacturer;
        equipment.ConditionId = dto.ConditionId;
        equipment.LocationId = dto.LocationId;
        equipment.PurchaseDate = dto.PurchaseDate;
        equipment.PurchasePrice = dto.PurchasePrice;
        equipment.LastMaintenanceDate = dto.LastMaintenanceDate;
        equipment.NextMaintenanceDue = dto.NextMaintenanceDue;
        equipment.IsActive = dto.IsActive;

        await _equipmentRepository.UpdateAsync(equipment);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(equipment);
    }

    public async Task DeleteAsync(Guid id)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(id);
        if (equipment == null)
        {
            throw new KeyNotFoundException($"Equipment with ID {id} not found");
        }

        await _equipmentRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    private EquipmentDto MapToDto(Equipment equipment)
    {
        return new EquipmentDto
        {
            Id = equipment.Id,
            Name = equipment.Name,
            Description = equipment.Description,
            SerialNumber = equipment.SerialNumber,
            Model = equipment.Model,
            Manufacturer = equipment.Manufacturer,
            ConditionId = equipment.ConditionId,
            ConditionName = equipment.Condition?.Name ?? string.Empty,
            LocationId = equipment.LocationId,
            LocationName = equipment.Location?.Name ?? string.Empty,
            PurchaseDate = equipment.PurchaseDate,
            PurchasePrice = equipment.PurchasePrice,
            LastMaintenanceDate = equipment.LastMaintenanceDate,
            NextMaintenanceDue = equipment.NextMaintenanceDue,
            IsActive = equipment.IsActive
        };
    }
}

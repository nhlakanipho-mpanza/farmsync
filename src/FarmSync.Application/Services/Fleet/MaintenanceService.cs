using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.Fleet;

namespace FarmSync.Application.Services.Fleet;

public class MaintenanceService : IMaintenanceService
{
    private readonly IMaintenanceRecordRepository _maintenanceRepository;
    private readonly IUnitOfWork _unitOfWork;

    public MaintenanceService(IMaintenanceRecordRepository maintenanceRepository, IUnitOfWork unitOfWork)
    {
        _maintenanceRepository = maintenanceRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<MaintenanceRecordDTO>> GetAllMaintenanceAsync()
    {
        var records = await _maintenanceRepository.GetAllAsync();
        return records.Select(MapToDto);
    }

    public async Task<MaintenanceRecordDTO?> GetMaintenanceByIdAsync(Guid id)
    {
        var record = await _maintenanceRepository.GetByIdAsync(id);
        return record != null ? MapToDto(record) : null;
    }

    public async Task<IEnumerable<MaintenanceRecordDTO>> GetByVehicleAsync(Guid vehicleId)
    {
        var records = await _maintenanceRepository.GetByVehicleAsync(vehicleId);
        return records.Select(MapToDto);
    }

    public async Task<IEnumerable<MaintenanceRecordDTO>> GetPendingMaintenanceAsync()
    {
        var records = await _maintenanceRepository.GetPendingMaintenanceAsync();
        return records.Select(MapToDto);
    }

    public async Task<IEnumerable<MaintenanceRecordDTO>> GetCompletedMaintenanceAsync(DateTime startDate, DateTime endDate)
    {
        var records = await _maintenanceRepository.GetCompletedMaintenanceAsync(startDate, endDate);
        return records.Select(MapToDto);
    }

    public async Task<MaintenanceRecordDTO> CreateMaintenanceAsync(CreateMaintenanceRecordDTO dto)
    {
        var record = new MaintenanceRecord
        {
            Id = Guid.NewGuid(),
            VehicleId = dto.VehicleId,
            MaintenanceTypeId = dto.MaintenanceTypeId,
            ScheduledDate = dto.ScheduledDate,
            Description = dto.Description,
            OdometerReading = dto.OdometerReading,
            EngineHours = dto.EngineHours,
            IsActive = true,
            IsCompleted = false,
            TotalCost = 0,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _maintenanceRepository.AddAsync(record);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _maintenanceRepository.GetByIdAsync(record.Id));
    }

    public async Task<MaintenanceRecordDTO> UpdateMaintenanceAsync(Guid id, UpdateMaintenanceRecordDTO dto)
    {
        var record = await _maintenanceRepository.GetByIdAsync(id);
        if (record == null)
        {
            throw new KeyNotFoundException($"Maintenance record with ID {id} not found.");
        }

        record.CompletedDate = dto.CompletedDate;
        record.PartsReplaced = dto.PartsReplaced;
        record.LaborCost = dto.LaborCost;
        record.PartsCost = dto.PartsCost;
        record.TotalCost = dto.TotalCost;
        record.MechanicNotes = dto.MechanicNotes;
        record.NextServiceOdometer = dto.NextServiceOdometer;
        record.NextServiceHours = dto.NextServiceHours;
        record.PerformedById = dto.PerformedById;
        record.IsCompleted = dto.IsCompleted;
        record.UpdatedAt = DateTime.UtcNow;

        await _maintenanceRepository.UpdateAsync(record);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _maintenanceRepository.GetByIdAsync(record.Id));
    }

    public async Task DeleteMaintenanceAsync(Guid id)
    {
        var record = await _maintenanceRepository.GetByIdAsync(id);
        if (record == null)
        {
            throw new KeyNotFoundException($"Maintenance record with ID {id} not found.");
        }

        await _maintenanceRepository.DeleteAsync(record.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task AutoScheduleMaintenanceAsync(Guid vehicleId)
    {
        // Auto-schedule logic can be implemented here
        // For now, just a placeholder
        await Task.CompletedTask;
    }

    private MaintenanceRecordDTO MapToDto(MaintenanceRecord record)
    {
        return new MaintenanceRecordDTO
        {
            Id = record.Id,
            VehicleId = record.VehicleId,
            VehicleRegistration = record.Vehicle?.RegistrationNumber,
            MaintenanceTypeId = record.MaintenanceTypeId,
            MaintenanceTypeName = record.MaintenanceType?.Name,
            PerformedById = record.PerformedById,
            PerformedByName = record.PerformedBy?.FullName,
            ScheduledDate = record.ScheduledDate,
            CompletedDate = record.CompletedDate,
            Description = record.Description,
            OdometerReading = record.OdometerReading,
            EngineHours = record.EngineHours,
            PartsReplaced = record.PartsReplaced,
            LaborCost = record.LaborCost,
            PartsCost = record.PartsCost,
            TotalCost = record.TotalCost,
            MechanicNotes = record.MechanicNotes,
            NextServiceOdometer = record.NextServiceOdometer,
            NextServiceHours = record.NextServiceHours,
            IsCompleted = record.IsCompleted,
            IsActive = record.IsActive,
            CreatedAt = record.CreatedAt
        };
    }
}

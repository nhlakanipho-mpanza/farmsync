using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.Fleet;

namespace FarmSync.Application.Services.Fleet;

public class FuelService : IFuelService
{
    private readonly IFuelLogRepository _fuelLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public FuelService(IFuelLogRepository fuelLogRepository, IUnitOfWork unitOfWork)
    {
        _fuelLogRepository = fuelLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<FuelLogDTO>> GetAllFuelLogsAsync()
    {
        var logs = await _fuelLogRepository.GetAllAsync();
        return logs.Select(MapToDto);
    }

    public async Task<FuelLogDTO?> GetFuelLogByIdAsync(Guid id)
    {
        var log = await _fuelLogRepository.GetByIdAsync(id);
        return log != null ? MapToDto(log) : null;
    }

    public async Task<IEnumerable<FuelLogDTO>> GetByVehicleAsync(Guid vehicleId)
    {
        var logs = await _fuelLogRepository.GetByVehicleAsync(vehicleId);
        return logs.Select(MapToDto);
    }

    public async Task<IEnumerable<FuelLogDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var logs = await _fuelLogRepository.GetByDateRangeAsync(startDate, endDate);
        return logs.Select(MapToDto);
    }

    public async Task<decimal> GetTotalFuelCostAsync(Guid vehicleId, DateTime? startDate = null, DateTime? endDate = null)
    {
        return await _fuelLogRepository.GetTotalFuelCostAsync(vehicleId, startDate, endDate);
    }

    public async Task<decimal> GetAverageFuelConsumptionAsync(Guid vehicleId)
    {
        return await _fuelLogRepository.GetAverageFuelConsumptionAsync(vehicleId);
    }

    public async Task<IEnumerable<FuelLogDTO>> GetAnomalousConsumptionAsync(Guid vehicleId)
    {
        var logs = await _fuelLogRepository.GetAnomalousConsumptionAsync(vehicleId);
        return logs.Select(MapToDto);
    }

    public async Task<FuelLogDTO> CreateFuelLogAsync(CreateFuelLogDTO dto)
    {
        var log = new FuelLog
        {
            Id = Guid.NewGuid(),
            VehicleId = dto.VehicleId,
            FilledById = dto.FilledById,
            FuelDate = dto.FuelDate,
            Quantity = dto.Quantity,
            UnitPrice = dto.UnitPrice,
            TotalCost = dto.Quantity * dto.UnitPrice,
            OdometerReading = dto.OdometerReading,
            Station = dto.Station,
            ReceiptNumber = dto.ReceiptNumber,
            IsFull = dto.IsFull,
            Notes = dto.Notes,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _fuelLogRepository.AddAsync(log);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _fuelLogRepository.GetByIdAsync(log.Id));
    }

    public async Task<FuelLogDTO> UpdateFuelLogAsync(Guid id, UpdateFuelLogDTO dto)
    {
        var log = await _fuelLogRepository.GetByIdAsync(id);
        if (log == null)
        {
            throw new KeyNotFoundException($"Fuel log with ID {id} not found.");
        }

        log.FuelDate = dto.FuelDate;
        log.Quantity = dto.Quantity;
        log.UnitPrice = dto.UnitPrice;
        log.TotalCost = dto.Quantity * dto.UnitPrice;
        log.OdometerReading = dto.OdometerReading;
        log.Station = dto.Station;
        log.ReceiptNumber = dto.ReceiptNumber;
        log.IsFull = dto.IsFull;
        log.Notes = dto.Notes;
        log.UpdatedAt = DateTime.UtcNow;

        await _fuelLogRepository.UpdateAsync(log);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _fuelLogRepository.GetByIdAsync(log.Id));
    }

    public async Task DeleteFuelLogAsync(Guid id)
    {
        var log = await _fuelLogRepository.GetByIdAsync(id);
        if (log == null)
        {
            throw new KeyNotFoundException($"Fuel log with ID {id} not found.");
        }

        await _fuelLogRepository.DeleteAsync(log.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    private FuelLogDTO MapToDto(FuelLog log)
    {
        return new FuelLogDTO
        {
            Id = log.Id,
            VehicleId = log.VehicleId,
            VehicleRegistration = log.Vehicle?.RegistrationNumber,
            FilledById = log.FilledById,
            FilledByName = log.FilledBy?.FullName,
            FuelDate = log.FuelDate,
            Quantity = log.Quantity,
            UnitPrice = log.UnitPrice,
            TotalCost = log.TotalCost,
            OdometerReading = log.OdometerReading,
            Station = log.Station,
            ReceiptNumber = log.ReceiptNumber,
            IsFull = log.IsFull,
            Notes = log.Notes,
            IsActive = log.IsActive,
            CreatedAt = log.CreatedAt
        };
    }
}

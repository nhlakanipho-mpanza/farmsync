using FarmSync.Application.DTOs.Fleet;

namespace FarmSync.Application.Interfaces.Fleet;

public interface IFuelService
{
    Task<IEnumerable<FuelLogDTO>> GetAllFuelLogsAsync();
    Task<FuelLogDTO?> GetFuelLogByIdAsync(Guid id);
    Task<IEnumerable<FuelLogDTO>> GetByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<FuelLogDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalFuelCostAsync(Guid vehicleId, DateTime? startDate = null, DateTime? endDate = null);
    Task<decimal> GetAverageFuelConsumptionAsync(Guid vehicleId);
    Task<IEnumerable<FuelLogDTO>> GetAnomalousConsumptionAsync(Guid vehicleId);
    Task<FuelLogDTO> CreateFuelLogAsync(CreateFuelLogDTO dto);
    Task<FuelLogDTO> UpdateFuelLogAsync(Guid id, UpdateFuelLogDTO dto);
    Task DeleteFuelLogAsync(Guid id);
}

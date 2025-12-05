using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Domain.Interfaces.Fleet;

public interface IFuelLogRepository : IRepository<FuelLog>
{
    Task<IEnumerable<FuelLog>> GetByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<FuelLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<decimal> GetTotalFuelCostAsync(Guid vehicleId, DateTime? startDate = null, DateTime? endDate = null);
    Task<decimal> GetAverageFuelConsumptionAsync(Guid vehicleId);
    Task<IEnumerable<FuelLog>> GetAnomalousConsumptionAsync(Guid vehicleId);
}

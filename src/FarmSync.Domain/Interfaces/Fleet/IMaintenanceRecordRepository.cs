using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Domain.Interfaces.Fleet;

public interface IMaintenanceRecordRepository : IRepository<MaintenanceRecord>
{
    Task<IEnumerable<MaintenanceRecord>> GetByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<MaintenanceRecord>> GetPendingMaintenanceAsync();
    Task<IEnumerable<MaintenanceRecord>> GetCompletedMaintenanceAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<MaintenanceRecord>> GetByMaintenanceTypeAsync(Guid maintenanceTypeId);
    Task<MaintenanceRecord?> GetLatestMaintenanceAsync(Guid vehicleId);
}

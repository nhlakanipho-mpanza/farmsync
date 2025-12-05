using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Domain.Interfaces.Fleet;

public interface ITripLogRepository : IRepository<TripLog>
{
    Task<IEnumerable<TripLog>> GetByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<TripLog>> GetByDriverAsync(Guid driverId);
    Task<IEnumerable<TripLog>> GetActiveTripsByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<TripLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<IEnumerable<TripLog>> GetByTaskAsync(Guid taskId);
    Task<TripLog?> GetCurrentTripAsync(Guid vehicleId);
}

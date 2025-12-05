using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Domain.Interfaces.Fleet;

public interface IGPSLocationRepository : IRepository<GPSLocation>
{
    Task<IEnumerable<GPSLocation>> GetByVehicleAsync(Guid vehicleId);
    Task<GPSLocation?> GetLatestLocationAsync(Guid vehicleId);
    Task<IEnumerable<GPSLocation>> GetLocationHistoryAsync(Guid vehicleId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<GPSLocation>> GetByTripAsync(Guid tripId);
    Task<IEnumerable<GPSLocation>> GetActiveVehicleLocationsAsync();
}

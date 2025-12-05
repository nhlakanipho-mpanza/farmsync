using FarmSync.Application.DTOs.Fleet;

namespace FarmSync.Application.Interfaces.Fleet;

public interface IGPSTrackingService
{
    Task<IEnumerable<GPSLocationDTO>> GetAllLocationsAsync();
    Task<GPSLocationDTO?> GetLocationByIdAsync(Guid id);
    Task<IEnumerable<GPSLocationDTO>> GetByVehicleAsync(Guid vehicleId);
    Task<GPSLocationDTO?> GetLatestLocationAsync(Guid vehicleId);
    Task<IEnumerable<GPSLocationDTO>> GetLocationHistoryAsync(Guid vehicleId, DateTime startDate, DateTime endDate);
    Task<IEnumerable<GPSLocationDTO>> GetByTripAsync(Guid tripId);
    Task<IEnumerable<GPSLocationDTO>> GetActiveVehicleLocationsAsync();
    Task<GPSLocationDTO> RecordLocationAsync(CreateGPSLocationDTO dto);
    Task DeleteLocationAsync(Guid id);
}

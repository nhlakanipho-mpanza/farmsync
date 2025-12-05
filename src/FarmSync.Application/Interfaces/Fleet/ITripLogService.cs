using FarmSync.Application.DTOs.Fleet;

namespace FarmSync.Application.Interfaces.Fleet;

public interface ITripLogService
{
    Task<IEnumerable<TripLogDTO>> GetAllTripsAsync();
    Task<TripLogDTO?> GetTripByIdAsync(Guid id);
    Task<IEnumerable<TripLogDTO>> GetByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<TripLogDTO>> GetByDriverAsync(Guid driverId);
    Task<IEnumerable<TripLogDTO>> GetActiveTripsByVehicleAsync(Guid vehicleId);
    Task<IEnumerable<TripLogDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate);
    Task<TripLogDTO?> GetCurrentTripAsync(Guid vehicleId);
    Task<TripLogDTO> CreateTripAsync(CreateTripLogDTO dto);
    Task<TripLogDTO> UpdateTripAsync(Guid id, UpdateTripLogDTO dto);
    Task DeleteTripAsync(Guid id);
}

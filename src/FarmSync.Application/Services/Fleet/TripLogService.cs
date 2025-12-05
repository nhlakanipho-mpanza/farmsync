using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.Fleet;

namespace FarmSync.Application.Services.Fleet;

public class TripLogService : ITripLogService
{
    private readonly ITripLogRepository _tripLogRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TripLogService(ITripLogRepository tripLogRepository, IUnitOfWork unitOfWork)
    {
        _tripLogRepository = tripLogRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TripLogDTO>> GetAllTripsAsync()
    {
        var trips = await _tripLogRepository.GetAllAsync();
        return trips.Select(MapToDto);
    }

    public async Task<TripLogDTO?> GetTripByIdAsync(Guid id)
    {
        var trip = await _tripLogRepository.GetByIdAsync(id);
        return trip != null ? MapToDto(trip) : null;
    }

    public async Task<IEnumerable<TripLogDTO>> GetByVehicleAsync(Guid vehicleId)
    {
        var trips = await _tripLogRepository.GetByVehicleAsync(vehicleId);
        return trips.Select(MapToDto);
    }

    public async Task<IEnumerable<TripLogDTO>> GetByDriverAsync(Guid driverId)
    {
        var trips = await _tripLogRepository.GetByDriverAsync(driverId);
        return trips.Select(MapToDto);
    }

    public async Task<IEnumerable<TripLogDTO>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        var trips = await _tripLogRepository.GetByDateRangeAsync(startDate, endDate);
        return trips.Select(MapToDto);
    }

    public async Task<IEnumerable<TripLogDTO>> GetActiveTripsByVehicleAsync(Guid vehicleId)
    {
        var trips = await _tripLogRepository.GetActiveTripsByVehicleAsync(vehicleId);
        return trips.Select(MapToDto);
    }

    public async Task<TripLogDTO?> GetCurrentTripAsync(Guid vehicleId)
    {
        var trips = await _tripLogRepository.GetActiveTripsByVehicleAsync(vehicleId);
        var currentTrip = trips.FirstOrDefault();
        return currentTrip != null ? MapToDto(currentTrip) : null;
    }

    public async Task<TripLogDTO> CreateTripAsync(CreateTripLogDTO dto)
    {
        var trip = new TripLog
        {
            Id = Guid.NewGuid(),
            VehicleId = dto.VehicleId,
            DriverId = dto.DriverId,
            TransportTaskId = dto.TransportTaskId,
            StartTime = dto.StartTime,
            StartOdometer = dto.StartOdometer,
            StartLocation = dto.StartLocation,
            StartLatitude = dto.StartLatitude,
            StartLongitude = dto.StartLongitude,
            Purpose = dto.Purpose,
            Notes = dto.Notes,
            IsActive = true,
            IsCompleted = false,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _tripLogRepository.AddAsync(trip);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _tripLogRepository.GetByIdAsync(trip.Id));
    }

    public async Task<TripLogDTO> UpdateTripAsync(Guid id, UpdateTripLogDTO dto)
    {
        var trip = await _tripLogRepository.GetByIdAsync(id);
        if (trip == null)
        {
            throw new KeyNotFoundException($"Trip log with ID {id} not found.");
        }

        trip.EndTime = dto.EndTime;
        trip.EndOdometer = dto.EndOdometer;
        trip.EndLocation = dto.EndLocation;
        trip.EndLatitude = dto.EndLatitude;
        trip.EndLongitude = dto.EndLongitude;
        trip.Notes = dto.Notes;
        trip.IsCompleted = dto.IsCompleted;
        
        if (dto.EndOdometer.HasValue && trip.StartOdometer > 0)
        {
            trip.DistanceTraveled = dto.EndOdometer.Value - trip.StartOdometer;
        }
        
        trip.UpdatedAt = DateTime.UtcNow;

        await _tripLogRepository.UpdateAsync(trip);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _tripLogRepository.GetByIdAsync(trip.Id));
    }

    public async Task DeleteTripAsync(Guid id)
    {
        var trip = await _tripLogRepository.GetByIdAsync(id);
        if (trip == null)
        {
            throw new KeyNotFoundException($"Trip log with ID {id} not found.");
        }

        await _tripLogRepository.DeleteAsync(trip.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    private TripLogDTO MapToDto(TripLog trip)
    {
        return new TripLogDTO
        {
            Id = trip.Id,
            VehicleId = trip.VehicleId,
            VehicleRegistration = trip.Vehicle?.RegistrationNumber,
            DriverId = trip.DriverId,
            DriverName = trip.Driver?.FullName,
            TransportTaskId = trip.TransportTaskId,
            StartTime = trip.StartTime,
            EndTime = trip.EndTime,
            StartOdometer = trip.StartOdometer,
            EndOdometer = trip.EndOdometer,
            DistanceTraveled = trip.DistanceTraveled,
            StartLocation = trip.StartLocation,
            EndLocation = trip.EndLocation,
            StartLatitude = trip.StartLatitude,
            StartLongitude = trip.StartLongitude,
            EndLatitude = trip.EndLatitude,
            EndLongitude = trip.EndLongitude,
            Purpose = trip.Purpose,
            Notes = trip.Notes,
            IsCompleted = trip.IsCompleted,
            IsActive = trip.IsActive,
            CreatedAt = trip.CreatedAt
        };
    }
}

using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.Fleet;

namespace FarmSync.Application.Services.Fleet;

public class GPSTrackingService : IGPSTrackingService
{
    private readonly IGPSLocationRepository _gpsRepository;
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GPSTrackingService(
        IGPSLocationRepository gpsRepository,
        IVehicleRepository vehicleRepository,
        IUnitOfWork unitOfWork)
    {
        _gpsRepository = gpsRepository;
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GPSLocationDTO>> GetAllLocationsAsync()
    {
        var locations = await _gpsRepository.GetAllAsync();
        return locations.Select(MapToDto);
    }

    public async Task<GPSLocationDTO?> GetLocationByIdAsync(Guid id)
    {
        var location = await _gpsRepository.GetByIdAsync(id);
        return location != null ? MapToDto(location) : null;
    }

    public async Task<IEnumerable<GPSLocationDTO>> GetByVehicleAsync(Guid vehicleId)
    {
        var locations = await _gpsRepository.GetByVehicleAsync(vehicleId);
        return locations.Select(MapToDto);
    }

    public async Task<GPSLocationDTO?> GetLatestLocationAsync(Guid vehicleId)
    {
        var location = await _gpsRepository.GetLatestLocationAsync(vehicleId);
        return location != null ? MapToDto(location) : null;
    }

    public async Task<IEnumerable<GPSLocationDTO>> GetLocationHistoryAsync(Guid vehicleId, DateTime startDate, DateTime endDate)
    {
        var locations = await _gpsRepository.GetLocationHistoryAsync(vehicleId, startDate, endDate);
        return locations.Select(MapToDto);
    }

    public async Task<IEnumerable<GPSLocationDTO>> GetByTripAsync(Guid tripId)
    {
        var tripLocations = await _gpsRepository.GetByTripAsync(tripId);
        return tripLocations.Select(MapToDto);
    }

    public async Task<IEnumerable<GPSLocationDTO>> GetActiveVehicleLocationsAsync()
    {
        var locations = await _gpsRepository.GetActiveVehicleLocationsAsync();
        return locations.Select(MapToDto);
    }

    public async Task<GPSLocationDTO> RecordLocationAsync(CreateGPSLocationDTO dto)
    {
        var location = new GPSLocation
        {
            Id = Guid.NewGuid(),
            VehicleId = dto.VehicleId,
            TripLogId = dto.TripLogId,
            Timestamp = dto.Timestamp,
            Latitude = dto.Latitude,
            Longitude = dto.Longitude,
            Speed = dto.Speed,
            Heading = dto.Heading,
            Altitude = dto.Altitude,
            Odometer = dto.Odometer,
            IsActive = true,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        await _gpsRepository.AddAsync(location);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _gpsRepository.GetByIdAsync(location.Id));
    }

    public async Task DeleteLocationAsync(Guid id)
    {
        var location = await _gpsRepository.GetByIdAsync(id);
        if (location != null)
        {
            await _gpsRepository.DeleteAsync(location.Id);
            await _unitOfWork.SaveChangesAsync();
        }
    }

    private GPSLocationDTO MapToDto(GPSLocation location)
    {
        return new GPSLocationDTO
        {
            Id = location.Id,
            VehicleId = location.VehicleId,
            VehicleRegistration = location.Vehicle?.RegistrationNumber,
            TripLogId = location.TripLogId,
            Timestamp = location.Timestamp,
            Latitude = location.Latitude,
            Longitude = location.Longitude,
            Speed = location.Speed,
            Heading = location.Heading,
            Altitude = location.Altitude,
            Odometer = location.Odometer
        };
    }
}

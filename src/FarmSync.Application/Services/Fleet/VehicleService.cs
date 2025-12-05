using FarmSync.Application.DTOs.Fleet;
using FarmSync.Application.Interfaces.Fleet;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.Fleet;

namespace FarmSync.Application.Services.Fleet;

public class VehicleService : IVehicleService
{
    private readonly IVehicleRepository _vehicleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public VehicleService(IVehicleRepository vehicleRepository, IUnitOfWork unitOfWork)
    {
        _vehicleRepository = vehicleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync()
    {
        var vehicles = await _vehicleRepository.GetAllAsync();
        return vehicles.Select(MapToDto);
    }

    public async Task<IEnumerable<VehicleDTO>> GetActiveVehiclesAsync()
    {
        var vehicles = await _vehicleRepository.GetActiveVehiclesAsync();
        return vehicles.Select(MapToDto);
    }

    public async Task<VehicleDTO?> GetVehicleByIdAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        return vehicle != null ? MapToDto(vehicle) : null;
    }

    public async Task<VehicleDTO?> GetByRegistrationNumberAsync(string registrationNumber)
    {
        var vehicle = await _vehicleRepository.GetByRegistrationNumberAsync(registrationNumber);
        return vehicle != null ? MapToDto(vehicle) : null;
    }

    public async Task<IEnumerable<VehicleDTO>> GetByTypeAsync(Guid vehicleTypeId)
    {
        var vehicles = await _vehicleRepository.GetByTypeAsync(vehicleTypeId);
        return vehicles.Select(MapToDto);
    }

    public async Task<IEnumerable<VehicleDTO>> GetByStatusAsync(Guid statusId)
    {
        var vehicles = await _vehicleRepository.GetByStatusAsync(statusId);
        return vehicles.Select(MapToDto);
    }

    public async Task<IEnumerable<VehicleDTO>> GetByDriverAsync(Guid driverId)
    {
        var vehicles = await _vehicleRepository.GetByDriverAsync(driverId);
        return vehicles.Select(MapToDto);
    }

    public async Task<IEnumerable<VehicleDTO>> GetVehiclesDueForMaintenanceAsync()
    {
        var vehicles = await _vehicleRepository.GetVehiclesDueForMaintenanceAsync();
        return vehicles.Select(MapToDto);
    }

    public async Task<IEnumerable<VehicleDTO>> GetVehiclesNeedingLicenseRenewalAsync(int daysThreshold = 30)
    {
        var vehicles = await _vehicleRepository.GetAllAsync();
        var today = DateTime.UtcNow.Date;
        var thresholdDate = today.AddDays(daysThreshold);

        var vehiclesNeedingRenewal = vehicles.Where(v =>
            v.IsActive &&
            v.LicenseDiskExpiryDate.HasValue &&
            v.LicenseDiskExpiryDate.Value.Date <= thresholdDate);

        return vehiclesNeedingRenewal.Select(MapToDto);
    }

    public async Task<VehicleDTO> CreateVehicleAsync(CreateVehicleDTO dto)
    {
        if (await _vehicleRepository.RegistrationNumberExistsAsync(dto.RegistrationNumber))
        {
            throw new InvalidOperationException($"Vehicle with registration {dto.RegistrationNumber} already exists");
        }

        var vehicle = new Vehicle
        {
            Id = Guid.NewGuid(),
            RegistrationNumber = dto.RegistrationNumber,
            Make = dto.Make,
            Model = dto.Model,
            Year = dto.Year,
            EngineNumber = dto.EngineNumber,
            ChassisNumber = dto.ChassisNumber,
            AssetNumber = dto.AssetNumber,
            CurrentOdometer = dto.CurrentOdometer,
            PurchaseDate = dto.PurchaseDate,
            PurchasePrice = dto.PurchasePrice,
            LastServiceDate = dto.LastServiceDate,
            LastServiceOdometer = dto.LastServiceOdometer,
            LastServiceType = dto.LastServiceType,
            LicenseDiskExpiryDate = dto.LicenseDiskExpiryDate,
            Notes = dto.Notes,
            VehicleTypeId = dto.VehicleTypeId,
            VehicleStatusId = dto.VehicleStatusId,
            FuelTypeId = dto.FuelTypeId,
            IsActive = true,
            CreatedAt = DateTime.UtcNow
        };

        // Calculate next service odometer
        CalculateNextServiceOdometer(vehicle);

        await _vehicleRepository.AddAsync(vehicle);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(vehicle);
    }

    public async Task<VehicleDTO> UpdateVehicleAsync(Guid id, UpdateVehicleDTO dto)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle == null)
        {
            throw new InvalidOperationException($"Vehicle with ID {id} not found");
        }

        if (vehicle.RegistrationNumber != dto.RegistrationNumber &&
            await _vehicleRepository.RegistrationNumberExistsAsync(dto.RegistrationNumber))
        {
            throw new InvalidOperationException($"Vehicle with registration {dto.RegistrationNumber} already exists");
        }

        vehicle.RegistrationNumber = dto.RegistrationNumber;
        vehicle.Make = dto.Make;
        vehicle.Model = dto.Model;
        vehicle.Year = dto.Year;
        vehicle.EngineNumber = dto.EngineNumber;
        vehicle.ChassisNumber = dto.ChassisNumber;
        vehicle.AssetNumber = dto.AssetNumber;
        vehicle.CurrentOdometer = dto.CurrentOdometer;
        vehicle.LastServiceDate = dto.LastServiceDate;
        vehicle.LastServiceOdometer = dto.LastServiceOdometer;
        vehicle.LastServiceType = dto.LastServiceType;
        vehicle.NextServiceOdometer = dto.NextServiceOdometer;
        vehicle.LicenseDiskExpiryDate = dto.LicenseDiskExpiryDate;
        vehicle.Notes = dto.Notes;
        vehicle.VehicleTypeId = dto.VehicleTypeId;
        vehicle.VehicleStatusId = dto.VehicleStatusId;
        vehicle.FuelTypeId = dto.FuelTypeId;
        vehicle.IsActive = dto.IsActive;
        vehicle.UpdatedAt = DateTime.UtcNow;

        // Recalculate next service if service info changed
        if (dto.LastServiceDate.HasValue || dto.LastServiceOdometer.HasValue || !string.IsNullOrEmpty(dto.LastServiceType))
        {
            CalculateNextServiceOdometer(vehicle);
        }

        await _vehicleRepository.UpdateAsync(vehicle);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(vehicle);
    }

    public async Task DeleteVehicleAsync(Guid id)
    {
        var vehicle = await _vehicleRepository.GetByIdAsync(id);
        if (vehicle == null)
        {
            throw new InvalidOperationException($"Vehicle with ID {id} not found");
        }

        vehicle.IsActive = false;
        vehicle.UpdatedAt = DateTime.UtcNow;

        await _vehicleRepository.UpdateAsync(vehicle);
        await _unitOfWork.SaveChangesAsync();
    }

    private VehicleDTO MapToDto(Vehicle vehicle)
    {
        return new VehicleDTO
        {
            Id = vehicle.Id,
            RegistrationNumber = vehicle.RegistrationNumber,
            Make = vehicle.Make,
            Model = vehicle.Model,
            Year = vehicle.Year,
            EngineNumber = vehicle.EngineNumber,
            ChassisNumber = vehicle.ChassisNumber,
            AssetNumber = vehicle.AssetNumber,
            CurrentOdometer = vehicle.CurrentOdometer,
            PurchaseDate = vehicle.PurchaseDate,
            PurchasePrice = vehicle.PurchasePrice,
            LastServiceDate = vehicle.LastServiceDate,
            LastServiceOdometer = vehicle.LastServiceOdometer,
            LastServiceType = vehicle.LastServiceType,
            NextServiceOdometer = vehicle.NextServiceOdometer,
            LicenseDiskExpiryDate = vehicle.LicenseDiskExpiryDate,
            Notes = vehicle.Notes,
            IsActive = vehicle.IsActive,
            VehicleTypeId = vehicle.VehicleTypeId,
            VehicleStatusId = vehicle.VehicleStatusId,
            FuelTypeId = vehicle.FuelTypeId,
            VehicleTypeName = vehicle.VehicleType?.Name,
            VehicleStatusName = vehicle.VehicleStatus?.Name,
            FuelTypeName = vehicle.FuelType?.Name,
            CreatedAt = vehicle.CreatedAt,
            UpdatedAt = vehicle.UpdatedAt
        };
    }

    private void CalculateNextServiceOdometer(Vehicle vehicle)
    {
        if (!vehicle.LastServiceOdometer.HasValue || string.IsNullOrEmpty(vehicle.LastServiceType))
        {
            return;
        }

        // Service intervals:
        // Minor Service: 10,000 - 15,000 KM (we'll use 12,500 as average)
        // Major Service: 30,000 - 45,000 KM (we'll use 37,500 as average)
        int serviceInterval = vehicle.LastServiceType.Equals("Major", StringComparison.OrdinalIgnoreCase)
            ? 37500  // Major service interval
            : 12500; // Minor service interval

        vehicle.NextServiceOdometer = vehicle.LastServiceOdometer.Value + serviceInterval;
    }
}

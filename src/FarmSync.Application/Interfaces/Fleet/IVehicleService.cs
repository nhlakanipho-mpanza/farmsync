using FarmSync.Application.DTOs.Fleet;

namespace FarmSync.Application.Interfaces.Fleet;

public interface IVehicleService
{
    Task<IEnumerable<VehicleDTO>> GetAllVehiclesAsync();
    Task<IEnumerable<VehicleDTO>> GetActiveVehiclesAsync();
    Task<VehicleDTO?> GetVehicleByIdAsync(Guid id);
    Task<VehicleDTO?> GetByRegistrationNumberAsync(string registrationNumber);
    Task<IEnumerable<VehicleDTO>> GetByTypeAsync(Guid vehicleTypeId);
    Task<IEnumerable<VehicleDTO>> GetByStatusAsync(Guid statusId);
    Task<IEnumerable<VehicleDTO>> GetByDriverAsync(Guid driverId);
    Task<IEnumerable<VehicleDTO>> GetVehiclesDueForMaintenanceAsync();
    Task<IEnumerable<VehicleDTO>> GetVehiclesNeedingLicenseRenewalAsync(int daysThreshold = 30);
    Task<VehicleDTO> CreateVehicleAsync(CreateVehicleDTO dto);
    Task<VehicleDTO> UpdateVehicleAsync(Guid id, UpdateVehicleDTO dto);
    Task DeleteVehicleAsync(Guid id);
}

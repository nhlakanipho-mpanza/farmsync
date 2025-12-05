using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Domain.Interfaces.Fleet;

public interface IVehicleRepository : IRepository<Vehicle>
{
    Task<Vehicle?> GetByRegistrationNumberAsync(string registrationNumber);
    Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync();
    Task<IEnumerable<Vehicle>> GetByTypeAsync(Guid vehicleTypeId);
    Task<IEnumerable<Vehicle>> GetByStatusAsync(Guid statusId);
    Task<IEnumerable<Vehicle>> GetByDriverAsync(Guid driverId);
    Task<IEnumerable<Vehicle>> GetVehiclesDueForMaintenanceAsync();
    Task<bool> RegistrationNumberExistsAsync(string registrationNumber);
}

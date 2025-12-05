using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces.Fleet;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.Fleet;

public class VehicleRepository : Repository<Vehicle>, IVehicleRepository
{
    public VehicleRepository(FarmSyncDbContext context) : base(context) { }

    public async Task<Vehicle?> GetByRegistrationNumberAsync(string registrationNumber)
    {
        return await _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.VehicleStatus)
            .Include(v => v.FuelType)
            .FirstOrDefaultAsync(v => v.RegistrationNumber == registrationNumber);
    }

    public async Task<IEnumerable<Vehicle>> GetActiveVehiclesAsync()
    {
        return await _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.VehicleStatus)
            .Include(v => v.FuelType)
            .Where(v => v.IsActive)
            .OrderBy(v => v.RegistrationNumber)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetByTypeAsync(Guid vehicleTypeId)
    {
        return await _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.VehicleStatus)
            .Where(v => v.VehicleTypeId == vehicleTypeId && v.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetByStatusAsync(Guid statusId)
    {
        return await _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.VehicleStatus)
            .Where(v => v.VehicleStatusId == statusId && v.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<Vehicle>> GetByDriverAsync(Guid driverId)
    {
        // Note: Primary driver tracking has been moved to the issuing module
        // This method is kept for backward compatibility but will return empty results
        return await Task.FromResult(Enumerable.Empty<Vehicle>());
    }

    public async Task<IEnumerable<Vehicle>> GetVehiclesDueForMaintenanceAsync()
    {
        return await _context.Vehicles
            .Include(v => v.VehicleType)
            .Include(v => v.VehicleStatus)
            .Where(v => v.IsActive && 
                v.NextServiceOdometer != null && v.CurrentOdometer >= v.NextServiceOdometer)
            .ToListAsync();
    }

    public async Task<bool> RegistrationNumberExistsAsync(string registrationNumber)
    {
        return await _context.Vehicles
            .AnyAsync(v => v.RegistrationNumber == registrationNumber);
    }
}

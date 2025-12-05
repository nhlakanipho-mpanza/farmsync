using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces.Fleet;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.Fleet;

public class GPSLocationRepository : Repository<GPSLocation>, IGPSLocationRepository
{
    public GPSLocationRepository(FarmSyncDbContext context) : base(context) { }

    public async Task<IEnumerable<GPSLocation>> GetByVehicleAsync(Guid vehicleId)
    {
        return await _context.GPSLocations
            .Include(g => g.Vehicle)
            .Include(g => g.TripLog)
            .Where(g => g.VehicleId == vehicleId && g.IsActive)
            .OrderByDescending(g => g.Timestamp)
            .ToListAsync();
    }

    public async Task<GPSLocation?> GetLatestLocationAsync(Guid vehicleId)
    {
        return await _context.GPSLocations
            .Include(g => g.Vehicle)
            .Where(g => g.VehicleId == vehicleId && g.IsActive)
            .OrderByDescending(g => g.Timestamp)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<GPSLocation>> GetLocationHistoryAsync(Guid vehicleId, DateTime startDate, DateTime endDate)
    {
        return await _context.GPSLocations
            .Include(g => g.Vehicle)
            .Include(g => g.TripLog)
            .Where(g => g.VehicleId == vehicleId && 
                        g.Timestamp >= startDate && 
                        g.Timestamp <= endDate && 
                        g.IsActive)
            .OrderBy(g => g.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<GPSLocation>> GetByTripAsync(Guid tripId)
    {
        return await _context.GPSLocations
            .Include(g => g.Vehicle)
            .Where(g => g.TripLogId == tripId && g.IsActive)
            .OrderBy(g => g.Timestamp)
            .ToListAsync();
    }

    public async Task<IEnumerable<GPSLocation>> GetActiveVehicleLocationsAsync()
    {
        var latestLocations = await _context.GPSLocations
            .Include(g => g.Vehicle)
                .ThenInclude(v => v.VehicleType)
            .Include(g => g.Vehicle.VehicleStatus)
            .Where(g => g.IsActive && g.Vehicle.IsActive)
            .GroupBy(g => g.VehicleId)
            .Select(g => g.OrderByDescending(l => l.Timestamp).FirstOrDefault())
            .Where(g => g != null && g.Timestamp >= DateTime.UtcNow.AddHours(-2))
            .ToListAsync();

        return latestLocations!;
    }
}

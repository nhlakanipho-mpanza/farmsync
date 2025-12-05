using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces.Fleet;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.Fleet;

public class TripLogRepository : Repository<TripLog>, ITripLogRepository
{
    public TripLogRepository(FarmSyncDbContext context) : base(context) { }

    public async Task<IEnumerable<TripLog>> GetByVehicleAsync(Guid vehicleId)
    {
        return await _context.TripLogs
            .Include(t => t.Vehicle)
            .Include(t => t.Driver)
            .Include(t => t.TransportTask)
            .Where(t => t.VehicleId == vehicleId && t.IsActive)
            .OrderByDescending(t => t.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<TripLog>> GetByDriverAsync(Guid driverId)
    {
        return await _context.TripLogs
            .Include(t => t.Vehicle)
            .Include(t => t.TransportTask)
            .Where(t => t.DriverId == driverId && t.IsActive)
            .OrderByDescending(t => t.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<TripLog>> GetActiveTripsByVehicleAsync(Guid vehicleId)
    {
        return await _context.TripLogs
            .Include(t => t.Vehicle)
            .Include(t => t.Driver)
            .Where(t => t.VehicleId == vehicleId && !t.IsCompleted && t.IsActive)
            .ToListAsync();
    }

    public async Task<IEnumerable<TripLog>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.TripLogs
            .Include(t => t.Vehicle)
            .Include(t => t.Driver)
            .Where(t => t.StartTime >= startDate && t.StartTime <= endDate && t.IsActive)
            .OrderByDescending(t => t.StartTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<TripLog>> GetByTaskAsync(Guid taskId)
    {
        return await _context.TripLogs
            .Include(t => t.Vehicle)
            .Include(t => t.Driver)
            .Where(t => t.TransportTaskId == taskId && t.IsActive)
            .ToListAsync();
    }

    public async Task<TripLog?> GetCurrentTripAsync(Guid vehicleId)
    {
        return await _context.TripLogs
            .Include(t => t.Vehicle)
            .Include(t => t.Driver)
            .Include(t => t.TransportTask)
            .Where(t => t.VehicleId == vehicleId && !t.IsCompleted && t.IsActive)
            .OrderByDescending(t => t.StartTime)
            .FirstOrDefaultAsync();
    }
}

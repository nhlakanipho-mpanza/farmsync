using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces.Fleet;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.Fleet;

public class MaintenanceRecordRepository : Repository<MaintenanceRecord>, IMaintenanceRecordRepository
{
    public MaintenanceRecordRepository(FarmSyncDbContext context) : base(context) { }

    public async Task<IEnumerable<MaintenanceRecord>> GetByVehicleAsync(Guid vehicleId)
    {
        return await _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .Include(m => m.MaintenanceType)
            .Include(m => m.PerformedBy)
            .Where(m => m.VehicleId == vehicleId && m.IsActive)
            .OrderByDescending(m => m.ScheduledDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<MaintenanceRecord>> GetPendingMaintenanceAsync()
    {
        return await _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .Include(m => m.MaintenanceType)
            .Where(m => !m.IsCompleted && m.IsActive)
            .OrderBy(m => m.ScheduledDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<MaintenanceRecord>> GetCompletedMaintenanceAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .Include(m => m.MaintenanceType)
            .Include(m => m.PerformedBy)
            .Where(m => m.IsCompleted && 
                        m.CompletedDate >= startDate && 
                        m.CompletedDate <= endDate && 
                        m.IsActive)
            .OrderByDescending(m => m.CompletedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<MaintenanceRecord>> GetByMaintenanceTypeAsync(Guid maintenanceTypeId)
    {
        return await _context.MaintenanceRecords
            .Include(m => m.Vehicle)
            .Include(m => m.MaintenanceType)
            .Where(m => m.MaintenanceTypeId == maintenanceTypeId && m.IsActive)
            .OrderByDescending(m => m.ScheduledDate)
            .ToListAsync();
    }

    public async Task<MaintenanceRecord?> GetLatestMaintenanceAsync(Guid vehicleId)
    {
        return await _context.MaintenanceRecords
            .Include(m => m.MaintenanceType)
            .Include(m => m.PerformedBy)
            .Where(m => m.VehicleId == vehicleId && m.IsCompleted && m.IsActive)
            .OrderByDescending(m => m.CompletedDate)
            .FirstOrDefaultAsync();
    }
}

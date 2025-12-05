using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces.Fleet;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.Fleet;

public class DriverAssignmentRepository : Repository<DriverAssignment>, IDriverAssignmentRepository
{
    public DriverAssignmentRepository(FarmSyncDbContext context) : base(context) { }

    public async Task<DriverAssignment?> GetCurrentAssignmentByVehicleAsync(Guid vehicleId)
    {
        var now = DateTime.UtcNow;
        return await _context.DriverAssignments
            .Include(da => da.Vehicle)
            .Include(da => da.Driver)
            .Include(da => da.AssignedBy)
            .FirstOrDefaultAsync(da => da.VehicleId == vehicleId && 
                                      (!da.EndDate.HasValue || da.EndDate.Value > now));
    }

    public async Task<IEnumerable<DriverAssignment>> GetAssignmentsByVehicleAsync(Guid vehicleId)
    {
        return await _context.DriverAssignments
            .Include(da => da.Vehicle)
            .Include(da => da.Driver)
            .Include(da => da.AssignedBy)
            .Where(da => da.VehicleId == vehicleId)
            .OrderByDescending(da => da.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<DriverAssignment>> GetAssignmentsByDriverAsync(Guid driverId)
    {
        return await _context.DriverAssignments
            .Include(da => da.Vehicle)
            .Include(da => da.Driver)
            .Include(da => da.AssignedBy)
            .Where(da => da.DriverId == driverId)
            .OrderByDescending(da => da.StartDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<DriverAssignment>> GetCurrentAssignmentsAsync()
    {
        var now = DateTime.UtcNow;
        return await _context.DriverAssignments
            .Include(da => da.Vehicle)
            .Include(da => da.Driver)
            .Include(da => da.AssignedBy)
            .Where(da => !da.EndDate.HasValue || da.EndDate.Value > now)
            .OrderBy(da => da.Vehicle.RegistrationNumber)
            .ToListAsync();
    }

    public async Task<bool> HasActiveAssignmentAsync(Guid vehicleId)
    {
        var now = DateTime.UtcNow;
        return await _context.DriverAssignments
            .AnyAsync(da => da.VehicleId == vehicleId && 
                           (!da.EndDate.HasValue || da.EndDate.Value > now));
    }
}

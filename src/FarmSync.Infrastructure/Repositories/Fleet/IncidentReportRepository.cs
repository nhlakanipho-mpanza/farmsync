using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.Fleet;
using FarmSync.Domain.Interfaces.Fleet;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.Fleet;

public class IncidentReportRepository : Repository<IncidentReport>, IIncidentReportRepository
{
    public IncidentReportRepository(FarmSyncDbContext context) : base(context) { }

    public async Task<IEnumerable<IncidentReport>> GetByVehicleAsync(Guid vehicleId)
    {
        return await _context.IncidentReports
            .Include(i => i.Vehicle)
            .Include(i => i.ReportedBy)
            .Include(i => i.AssignedTo)
            .Where(i => i.VehicleId == vehicleId && i.IsActive)
            .OrderByDescending(i => i.IncidentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IncidentReport>> GetByStatusAsync(string status)
    {
        return await _context.IncidentReports
            .Include(i => i.Vehicle)
            .Include(i => i.ReportedBy)
            .Include(i => i.AssignedTo)
            .Where(i => i.Status == status && i.IsActive)
            .OrderByDescending(i => i.IncidentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IncidentReport>> GetByDriverAsync(Guid driverId)
    {
        return await _context.IncidentReports
            .Include(i => i.Vehicle)
            .Include(i => i.ReportedBy)
            .Where(i => i.ReportedById == driverId && i.IsActive)
            .OrderByDescending(i => i.IncidentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IncidentReport>> GetBySeverityAsync(string severity)
    {
        return await _context.IncidentReports
            .Include(i => i.Vehicle)
            .Include(i => i.ReportedBy)
            .Include(i => i.AssignedTo)
            .Where(i => i.Severity == severity && i.IsActive)
            .OrderByDescending(i => i.IncidentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IncidentReport>> GetUnresolvedIncidentsAsync()
    {
        return await _context.IncidentReports
            .Include(i => i.Vehicle)
            .Include(i => i.ReportedBy)
            .Include(i => i.AssignedTo)
            .Where(i => i.Status != "Resolved" && i.IsActive)
            .OrderByDescending(i => i.IncidentDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<IncidentReport>> GetByDateRangeAsync(DateTime startDate, DateTime endDate)
    {
        return await _context.IncidentReports
            .Include(i => i.Vehicle)
            .Include(i => i.ReportedBy)
            .Include(i => i.AssignedTo)
            .Where(i => i.IncidentDate >= startDate && i.IncidentDate <= endDate && i.IsActive)
            .OrderByDescending(i => i.IncidentDate)
            .ToListAsync();
    }
}

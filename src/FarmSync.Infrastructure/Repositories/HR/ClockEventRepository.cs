using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces.HR;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.HR;

public class ClockEventRepository : Repository<ClockEvent>, IClockEventRepository
{
    public ClockEventRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<ClockEvent>> GetByEmployeeAsync(Guid employeeId, DateOnly? fromDate = null, DateOnly? toDate = null)
    {
        var query = _context.ClockEvents
            .Include(c => c.Employee)
            .Include(c => c.Team)
            .Where(c => c.EmployeeId == employeeId);

        if (fromDate.HasValue)
        {
            var fromDateTime = fromDate.Value.ToDateTime(TimeOnly.MinValue);
            query = query.Where(c => c.EventTime >= fromDateTime);
        }

        if (toDate.HasValue)
        {
            var toDateTime = toDate.Value.ToDateTime(TimeOnly.MaxValue);
            query = query.Where(c => c.EventTime <= toDateTime);
        }

        return await query
            .OrderByDescending(c => c.EventTime)
            .ToListAsync();
    }

    public async Task<IEnumerable<ClockEvent>> GetByTeamAsync(Guid teamId, DateOnly date)
    {
        var startDate = date.ToDateTime(TimeOnly.MinValue);
        var endDate = date.ToDateTime(TimeOnly.MaxValue);

        return await _context.ClockEvents
            .Include(c => c.Employee)
            .Include(c => c.Team)
            .Where(c => c.TeamId == teamId && c.EventTime >= startDate && c.EventTime <= endDate)
            .OrderBy(c => c.EventTime)
            .ToListAsync();
    }

    public async Task<ClockEvent?> GetLastEventAsync(Guid employeeId)
    {
        return await _context.ClockEvents
            .Include(c => c.Employee)
            .Include(c => c.Team)
            .Where(c => c.EmployeeId == employeeId)
            .OrderByDescending(c => c.EventTime)
            .FirstOrDefaultAsync();
    }

    public async Task<IEnumerable<ClockEvent>> GetEventsByDateAsync(DateOnly date)
    {
        var startDate = date.ToDateTime(TimeOnly.MinValue);
        var endDate = date.ToDateTime(TimeOnly.MaxValue);

        return await _context.ClockEvents
            .Include(c => c.Employee)
            .Include(c => c.Team)
            .Where(c => c.EventTime >= startDate && c.EventTime <= endDate)
            .OrderBy(c => c.EventTime)
            .ToListAsync();
    }
}

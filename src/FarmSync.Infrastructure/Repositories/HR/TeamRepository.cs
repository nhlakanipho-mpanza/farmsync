using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces.HR;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.HR;

public class TeamRepository : Repository<Team>, ITeamRepository
{
    public TeamRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Team>> GetActiveTeamsAsync()
    {
        return await _context.Teams
            .Include(t => t.TeamLeader)
            .Include(t => t.TeamType)
            .Include(t => t.Members)
                .ThenInclude(m => m.Employee)
            .Where(t => t.IsActive)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Team>> GetTeamsByTypeAsync(Guid teamTypeId)
    {
        return await _context.Teams
            .Include(t => t.TeamLeader)
            .Include(t => t.TeamType)
            .Where(t => t.TeamTypeId == teamTypeId)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Team>> GetTeamsByLeaderAsync(Guid employeeId)
    {
        return await _context.Teams
            .Include(t => t.TeamLeader)
            .Include(t => t.TeamType)
            .Include(t => t.Members)
                .ThenInclude(m => m.Employee)
            .Where(t => t.TeamLeaderId == employeeId)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<Team?> GetWithMembersAsync(Guid id)
    {
        return await _context.Teams
            .Include(t => t.TeamLeader)
            .Include(t => t.TeamType)
            .Include(t => t.Members)
                .ThenInclude(m => m.Employee)
                    .ThenInclude(e => e.Position)
            .FirstOrDefaultAsync(t => t.Id == id);
    }
}

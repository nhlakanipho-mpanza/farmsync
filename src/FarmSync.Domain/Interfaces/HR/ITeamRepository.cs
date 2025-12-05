using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Interfaces.HR;

public interface ITeamRepository : IRepository<Team>
{
    Task<IEnumerable<Team>> GetActiveTeamsAsync();
    Task<IEnumerable<Team>> GetTeamsByTypeAsync(Guid teamTypeId);
    Task<IEnumerable<Team>> GetTeamsByLeaderAsync(Guid employeeId);
    Task<Team?> GetWithMembersAsync(Guid id);
}

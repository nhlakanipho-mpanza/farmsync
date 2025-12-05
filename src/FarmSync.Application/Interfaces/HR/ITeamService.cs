using FarmSync.Application.DTOs.HR;

namespace FarmSync.Application.Interfaces.HR;

public interface ITeamService
{
    Task<IEnumerable<TeamDTO>> GetAllTeamsAsync();
    Task<IEnumerable<TeamDTO>> GetActiveTeamsAsync();
    Task<TeamDTO?> GetTeamByIdAsync(Guid id);
    Task<TeamDTO> CreateTeamAsync(CreateTeamDTO dto);
    Task<TeamDTO> UpdateTeamAsync(Guid id, UpdateTeamDTO dto);
    Task DeleteTeamAsync(Guid id);
    Task<TeamMemberDTO> AddTeamMemberAsync(CreateTeamMemberDTO dto);
    Task<TeamMemberDTO> UpdateTeamMemberAsync(Guid id, UpdateTeamMemberDTO dto);
    Task RemoveTeamMemberAsync(Guid id);
    Task<IEnumerable<TeamMemberDTO>> GetTeamMembersAsync(Guid teamId);
}

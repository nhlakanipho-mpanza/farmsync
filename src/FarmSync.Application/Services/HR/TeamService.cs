using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.HR;

namespace FarmSync.Application.Services.HR;

public class TeamService : ITeamService
{
    private readonly ITeamRepository _teamRepository;
    private readonly IRepository<TeamMember> _teamMemberRepository;
    private readonly IUnitOfWork _unitOfWork;

    public TeamService(
        ITeamRepository teamRepository,
        IRepository<TeamMember> teamMemberRepository,
        IUnitOfWork unitOfWork)
    {
        _teamRepository = teamRepository;
        _teamMemberRepository = teamMemberRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<TeamDTO>> GetAllTeamsAsync()
    {
        var teams = await _teamRepository.GetAllAsync();
        return teams.Select(MapToDto);
    }

    public async Task<IEnumerable<TeamDTO>> GetActiveTeamsAsync()
    {
        var teams = await _teamRepository.GetActiveTeamsAsync();
        return teams.Select(MapToDto);
    }

    public async Task<TeamDTO?> GetTeamByIdAsync(Guid id)
    {
        var team = await _teamRepository.GetWithMembersAsync(id);
        return team != null ? MapToDto(team) : null;
    }

    public async Task<TeamDTO> CreateTeamAsync(CreateTeamDTO dto)
    {
        var team = new Team
        {
            Id = Guid.NewGuid(),
            Name = dto.Name,
            TeamLeaderId = dto.TeamLeaderId,
            TeamTypeId = dto.TeamTypeId,
            Description = dto.Description,
            IsActive = true
        };

        await _teamRepository.AddAsync(team);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(team);
    }

    public async Task<TeamDTO> UpdateTeamAsync(Guid id, UpdateTeamDTO dto)
    {
        var team = await _teamRepository.GetByIdAsync(id);
        if (team == null)
        {
            throw new KeyNotFoundException($"Team with ID {id} not found");
        }

        team.Name = dto.Name;
        team.TeamLeaderId = dto.TeamLeaderId;
        team.TeamTypeId = dto.TeamTypeId;
        team.Description = dto.Description;
        team.IsActive = dto.IsActive;

        await _teamRepository.UpdateAsync(team);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(team);
    }

    public async Task DeleteTeamAsync(Guid id)
    {
        await _teamRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<TeamMemberDTO> AddTeamMemberAsync(CreateTeamMemberDTO dto)
    {
        var member = new TeamMember
        {
            Id = Guid.NewGuid(),
            TeamId = dto.TeamId,
            EmployeeId = dto.EmployeeId,
            AssignedDate = dto.AssignedDate,
            EndDate = dto.EndDate,
            IsPermanent = dto.IsPermanent,
            Notes = dto.Notes
        };

        await _teamMemberRepository.AddAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return new TeamMemberDTO
        {
            Id = member.Id,
            TeamId = member.TeamId,
            EmployeeId = member.EmployeeId,
            AssignedDate = member.AssignedDate,
            EndDate = member.EndDate,
            IsPermanent = member.IsPermanent,
            Notes = member.Notes
        };
    }

    public async Task<TeamMemberDTO> UpdateTeamMemberAsync(Guid id, UpdateTeamMemberDTO dto)
    {
        var member = await _teamMemberRepository.GetByIdAsync(id);
        if (member == null)
        {
            throw new KeyNotFoundException($"Team member with ID {id} not found");
        }

        member.EndDate = dto.EndDate;
        member.IsPermanent = dto.IsPermanent;
        member.Notes = dto.Notes;

        await _teamMemberRepository.UpdateAsync(member);
        await _unitOfWork.SaveChangesAsync();

        return new TeamMemberDTO
        {
            Id = member.Id,
            TeamId = member.TeamId,
            EmployeeId = member.EmployeeId,
            AssignedDate = member.AssignedDate,
            EndDate = member.EndDate,
            IsPermanent = member.IsPermanent,
            Notes = member.Notes
        };
    }

    public async Task RemoveTeamMemberAsync(Guid id)
    {
        await _teamMemberRepository.DeleteAsync(id);
        await _unitOfWork.SaveChangesAsync();
    }

    public async Task<IEnumerable<TeamMemberDTO>> GetTeamMembersAsync(Guid teamId)
    {
        var members = (await _teamMemberRepository.GetAllAsync())
            .Where(m => m.TeamId == teamId);

        return members.Select(m => new TeamMemberDTO
        {
            Id = m.Id,
            TeamId = m.TeamId,
            EmployeeId = m.EmployeeId,
            EmployeeName = m.Employee?.FullName ?? "",
            EmployeeNumber = m.Employee?.EmployeeNumber ?? "",
            AssignedDate = m.AssignedDate,
            EndDate = m.EndDate,
            IsPermanent = m.IsPermanent,
            Notes = m.Notes
        });
    }

    private static TeamDTO MapToDto(Team team)
    {
        return new TeamDTO
        {
            Id = team.Id,
            Name = team.Name,
            TeamLeaderId = team.TeamLeaderId,
            TeamLeaderName = team.TeamLeader?.FullName,
            TeamTypeId = team.TeamTypeId,
            TeamTypeName = team.TeamType?.Name,
            Description = team.Description,
            IsActive = team.IsActive,
            MemberCount = team.Members?.Count ?? 0,
            CreatedAt = team.CreatedAt,
            UpdatedAt = team.UpdatedAt
        };
    }
}

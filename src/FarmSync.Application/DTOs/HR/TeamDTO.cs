namespace FarmSync.Application.DTOs.HR;

public class TeamDTO
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public Guid? TeamLeaderId { get; set; }
    public string? TeamLeaderName { get; set; }
    public Guid? TeamTypeId { get; set; }
    public string? TeamTypeName { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
    public int MemberCount { get; set; }
    public List<TeamMemberDTO> Members { get; set; } = new();
    public DateTime CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
}

public class CreateTeamDTO
{
    public string Name { get; set; } = string.Empty;
    public Guid? TeamLeaderId { get; set; }
    public Guid? TeamTypeId { get; set; }
    public string? Description { get; set; }
}

public class UpdateTeamDTO
{
    public string Name { get; set; } = string.Empty;
    public Guid? TeamLeaderId { get; set; }
    public Guid? TeamTypeId { get; set; }
    public string? Description { get; set; }
    public bool IsActive { get; set; }
}

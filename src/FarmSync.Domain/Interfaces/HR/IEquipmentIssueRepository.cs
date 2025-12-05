using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Interfaces.HR;

public interface IEquipmentIssueRepository : IRepository<EquipmentIssue>
{
    Task<IEnumerable<EquipmentIssue>> GetByStatusAsync(Guid statusId);
    Task<IEnumerable<EquipmentIssue>> GetByTeamAsync(Guid teamId);
    Task<IEnumerable<EquipmentIssue>> GetByWorkTaskAsync(Guid workTaskId);
    Task<IEnumerable<EquipmentIssue>> GetPendingApprovalsAsync();
    Task<IEnumerable<EquipmentIssue>> GetOverdueReturnsAsync();
    Task<string> GenerateIssueNumberAsync();
}

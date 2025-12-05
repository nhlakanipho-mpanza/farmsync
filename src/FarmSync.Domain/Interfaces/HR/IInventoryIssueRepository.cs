using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Interfaces.HR;

public interface IInventoryIssueRepository : IRepository<InventoryIssue>
{
    Task<IEnumerable<InventoryIssue>> GetByStatusAsync(Guid statusId);
    Task<IEnumerable<InventoryIssue>> GetByTeamAsync(Guid teamId);
    Task<IEnumerable<InventoryIssue>> GetByWorkTaskAsync(Guid workTaskId);
    Task<IEnumerable<InventoryIssue>> GetPendingApprovalsAsync();
    Task<string> GenerateIssueNumberAsync();
}

using FarmSync.Domain.Entities.HR;

namespace FarmSync.Domain.Interfaces.HR;

public interface IWorkTaskRepository : IRepository<WorkTask>
{
    Task<IEnumerable<WorkTask>> GetByStatusAsync(Guid statusId);
    Task<IEnumerable<WorkTask>> GetByTeamAsync(Guid teamId);
    Task<IEnumerable<WorkTask>> GetByEmployeeAsync(Guid employeeId);
    Task<IEnumerable<WorkTask>> GetByWorkAreaAsync(Guid workAreaId);
    Task<IEnumerable<WorkTask>> GetScheduledTasksAsync(DateOnly date);
    Task<IEnumerable<WorkTask>> GetTasksWithIssuesAsync(Guid taskId);
}

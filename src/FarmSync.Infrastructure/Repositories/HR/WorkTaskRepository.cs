using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces.HR;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.HR;

public class WorkTaskRepository : Repository<WorkTask>, IWorkTaskRepository
{
    public WorkTaskRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<WorkTask>> GetByStatusAsync(Guid statusId)
    {
        return await _context.WorkTasks
            .Include(t => t.WorkArea)
            .Include(t => t.Team)
            .Include(t => t.Employee)
            .Include(t => t.TaskStatus)
            .Where(t => t.TaskStatusId == statusId)
            .OrderByDescending(t => t.ScheduledDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkTask>> GetByTeamAsync(Guid teamId)
    {
        return await _context.WorkTasks
            .Include(t => t.WorkArea)
            .Include(t => t.Team)
            .Include(t => t.Employee)
            .Include(t => t.TaskStatus)
            .Where(t => t.TeamId == teamId)
            .OrderByDescending(t => t.ScheduledDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkTask>> GetByEmployeeAsync(Guid employeeId)
    {
        return await _context.WorkTasks
            .Include(t => t.WorkArea)
            .Include(t => t.Team)
            .Include(t => t.Employee)
            .Include(t => t.TaskStatus)
            .Where(t => t.EmployeeId == employeeId)
            .OrderByDescending(t => t.ScheduledDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkTask>> GetByWorkAreaAsync(Guid workAreaId)
    {
        return await _context.WorkTasks
            .Include(t => t.WorkArea)
            .Include(t => t.Team)
            .Include(t => t.Employee)
            .Include(t => t.TaskStatus)
            .Where(t => t.WorkAreaId == workAreaId)
            .OrderByDescending(t => t.ScheduledDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkTask>> GetScheduledTasksAsync(DateOnly date)
    {
        return await _context.WorkTasks
            .Include(t => t.WorkArea)
            .Include(t => t.Team)
            .Include(t => t.Employee)
            .Include(t => t.TaskStatus)
            .Where(t => t.ScheduledDate == date)
            .OrderBy(t => t.WorkArea!.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<WorkTask>> GetTasksWithIssuesAsync(Guid taskId)
    {
        return await _context.WorkTasks
            .Include(t => t.InventoryIssues)
                .ThenInclude(i => i.InventoryItem)
            .Include(t => t.EquipmentIssues)
                .ThenInclude(e => e.Equipment)
            .Where(t => t.Id == taskId)
            .ToListAsync();
    }
}

using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces.HR;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.HR;

public class InventoryIssueRepository : Repository<InventoryIssue>, IInventoryIssueRepository
{
    public InventoryIssueRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<InventoryIssue>> GetByStatusAsync(Guid statusId)
    {
        return await _context.InventoryIssues
            .Include(i => i.InventoryItem)
            .Include(i => i.WorkTask)
            .Include(i => i.Team)
            .Include(i => i.Employee)
            .Include(i => i.IssueStatus)
            .Where(i => i.IssueStatusId == statusId)
            .OrderByDescending(i => i.IssuedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryIssue>> GetByTeamAsync(Guid teamId)
    {
        return await _context.InventoryIssues
            .Include(i => i.InventoryItem)
            .Include(i => i.WorkTask)
            .Include(i => i.Team)
            .Include(i => i.Employee)
            .Include(i => i.IssueStatus)
            .Where(i => i.TeamId == teamId)
            .OrderByDescending(i => i.IssuedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryIssue>> GetByWorkTaskAsync(Guid workTaskId)
    {
        return await _context.InventoryIssues
            .Include(i => i.InventoryItem)
            .Include(i => i.WorkTask)
            .Include(i => i.Team)
            .Include(i => i.Employee)
            .Include(i => i.IssueStatus)
            .Where(i => i.WorkTaskId == workTaskId)
            .OrderByDescending(i => i.IssuedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<InventoryIssue>> GetPendingApprovalsAsync()
    {
        var pendingStatus = await _context.IssueStatuses
            .FirstOrDefaultAsync(s => s.Name == "Pending");

        if (pendingStatus == null)
            return new List<InventoryIssue>();

        return await _context.InventoryIssues
            .Include(i => i.InventoryItem)
            .Include(i => i.WorkTask)
            .Include(i => i.Team)
            .Include(i => i.Employee)
            .Include(i => i.IssueStatus)
            .Where(i => i.IssueStatusId == pendingStatus.Id)
            .OrderBy(i => i.IssuedDate)
            .ToListAsync();
    }

    public async Task<string> GenerateIssueNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"INV-{year}-";
        
        var lastIssue = await _context.InventoryIssues
            .Where(i => i.IssueNumber.StartsWith(prefix))
            .OrderByDescending(i => i.IssueNumber)
            .FirstOrDefaultAsync();

        if (lastIssue == null)
            return $"{prefix}0001";

        var lastNumber = int.Parse(lastIssue.IssueNumber.Substring(prefix.Length));
        return $"{prefix}{(lastNumber + 1):D4}";
    }
}

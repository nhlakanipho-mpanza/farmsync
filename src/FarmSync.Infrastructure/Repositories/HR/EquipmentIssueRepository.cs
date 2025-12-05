using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces.HR;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.HR;

public class EquipmentIssueRepository : Repository<EquipmentIssue>, IEquipmentIssueRepository
{
    public EquipmentIssueRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<EquipmentIssue>> GetByStatusAsync(Guid statusId)
    {
        return await _context.EquipmentIssues
            .Include(e => e.Equipment)
            .Include(e => e.WorkTask)
            .Include(e => e.Team)
            .Include(e => e.Employee)
            .Include(e => e.IssueStatus)
            .Where(e => e.IssueStatusId == statusId)
            .OrderByDescending(e => e.IssuedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<EquipmentIssue>> GetByTeamAsync(Guid teamId)
    {
        return await _context.EquipmentIssues
            .Include(e => e.Equipment)
            .Include(e => e.WorkTask)
            .Include(e => e.Team)
            .Include(e => e.Employee)
            .Include(e => e.IssueStatus)
            .Where(e => e.TeamId == teamId)
            .OrderByDescending(e => e.IssuedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<EquipmentIssue>> GetByWorkTaskAsync(Guid workTaskId)
    {
        return await _context.EquipmentIssues
            .Include(e => e.Equipment)
            .Include(e => e.WorkTask)
            .Include(e => e.Team)
            .Include(e => e.Employee)
            .Include(e => e.IssueStatus)
            .Where(e => e.WorkTaskId == workTaskId)
            .OrderByDescending(e => e.IssuedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<EquipmentIssue>> GetPendingApprovalsAsync()
    {
        var pendingStatus = await _context.IssueStatuses
            .FirstOrDefaultAsync(s => s.Name == "Pending");

        if (pendingStatus == null)
            return new List<EquipmentIssue>();

        return await _context.EquipmentIssues
            .Include(e => e.Equipment)
            .Include(e => e.WorkTask)
            .Include(e => e.Team)
            .Include(e => e.Employee)
            .Include(e => e.IssueStatus)
            .Where(e => e.IssueStatusId == pendingStatus.Id)
            .OrderBy(e => e.IssuedDate)
            .ToListAsync();
    }

    public async Task<IEnumerable<EquipmentIssue>> GetOverdueReturnsAsync()
    {
        var issuedStatus = await _context.IssueStatuses
            .FirstOrDefaultAsync(s => s.Name == "Issued");

        if (issuedStatus == null)
            return new List<EquipmentIssue>();

        return await _context.EquipmentIssues
            .Include(e => e.Equipment)
            .Include(e => e.WorkTask)
            .Include(e => e.Team)
            .Include(e => e.Employee)
            .Include(e => e.IssueStatus)
            .Where(e => e.IssueStatusId == issuedStatus.Id && 
                       e.ExpectedReturnDate.HasValue && 
                       e.ExpectedReturnDate.Value < DateTime.UtcNow)
            .OrderBy(e => e.ExpectedReturnDate)
            .ToListAsync();
    }

    public async Task<string> GenerateIssueNumberAsync()
    {
        var year = DateTime.UtcNow.Year;
        var prefix = $"EQP-{year}-";
        
        var lastIssue = await _context.EquipmentIssues
            .Where(e => e.IssueNumber.StartsWith(prefix))
            .OrderByDescending(e => e.IssueNumber)
            .FirstOrDefaultAsync();

        if (lastIssue == null)
            return $"{prefix}0001";

        var lastNumber = int.Parse(lastIssue.IssueNumber.Substring(prefix.Length));
        return $"{prefix}{(lastNumber + 1):D4}";
    }
}

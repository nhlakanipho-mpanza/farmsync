using Microsoft.EntityFrameworkCore;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Interfaces.HR;
using FarmSync.Infrastructure.Data;

namespace FarmSync.Infrastructure.Repositories.HR;

public class TaskTemplateRepository : ITaskTemplateRepository
{
    private readonly FarmSyncDbContext _context;

    public TaskTemplateRepository(FarmSyncDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<TaskTemplate>> GetAllAsync()
    {
        return await _context.TaskTemplates
            .Include(t => t.ChecklistItems.OrderBy(c => c.Sequence))
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskTemplate>> GetActivTemplatesAsync()
    {
        return await _context.TaskTemplates
            .Include(t => t.ChecklistItems.OrderBy(c => c.Sequence))
            .Where(t => t.IsActive)
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskTemplate>> GetByCategoryAsync(string category)
    {
        return await _context.TaskTemplates
            .Include(t => t.ChecklistItems.OrderBy(c => c.Sequence))
            .Where(t => t.IsActive && t.Category == category)
            .OrderBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<TaskTemplate>> GetRecurringTemplatesAsync()
    {
        return await _context.TaskTemplates
            .Include(t => t.ChecklistItems.OrderBy(c => c.Sequence))
            .Where(t => t.IsActive && t.IsRecurring)
            .OrderBy(t => t.Category)
            .ThenBy(t => t.Name)
            .ToListAsync();
    }

    public async Task<TaskTemplate?> GetByIdAsync(Guid id)
    {
        return await _context.TaskTemplates
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskTemplate?> GetByIdWithChecklistAsync(Guid id)
    {
        return await _context.TaskTemplates
            .Include(t => t.ChecklistItems.OrderBy(c => c.Sequence))
            .Include(t => t.InventoryItems.OrderBy(i => i.Sequence))
                .ThenInclude(i => i.InventoryItem)
            .FirstOrDefaultAsync(t => t.Id == id);
    }

    public async Task<TaskTemplate> CreateAsync(TaskTemplate template)
    {
        _context.TaskTemplates.Add(template);
        await _context.SaveChangesAsync();
        return template;
    }

    public async Task UpdateAsync(TaskTemplate template)
    {
        _context.TaskTemplates.Update(template);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(Guid id)
    {
        var template = await GetByIdAsync(id);
        if (template != null)
        {
            _context.TaskTemplates.Remove(template);
            await _context.SaveChangesAsync();
        }
    }
}

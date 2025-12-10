using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Interfaces;
using FarmSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FarmSync.Infrastructure.Repositories;

public class EquipmentRepository : IEquipmentRepository
{
    private readonly FarmSyncDbContext _context;

    public EquipmentRepository(FarmSyncDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Equipment>> GetAllAsync()
    {
        return await _context.Equipment
            .Include(e => e.Condition)
            .Include(e => e.Location)
            .Include(e => e.MaintenanceRecords)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<Equipment?> GetByIdAsync(Guid id)
    {
        return await _context.Equipment
            .Include(e => e.Condition)
            .Include(e => e.Location)
            .Include(e => e.MaintenanceRecords)
            .FirstOrDefaultAsync(e => e.Id == id);
    }

    public async Task<IEnumerable<Equipment>> GetActiveAsync()
    {
        return await _context.Equipment
            .Include(e => e.Condition)
            .Include(e => e.Location)
            .Where(e => e.IsActive)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Equipment>> GetByConditionAsync(Guid conditionId)
    {
        return await _context.Equipment
            .Include(e => e.Condition)
            .Include(e => e.Location)
            .Where(e => e.ConditionId == conditionId)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task<IEnumerable<Equipment>> GetByLocationAsync(Guid locationId)
    {
        return await _context.Equipment
            .Include(e => e.Condition)
            .Include(e => e.Location)
            .Where(e => e.LocationId == locationId)
            .OrderBy(e => e.Name)
            .ToListAsync();
    }

    public async Task AddAsync(Equipment equipment)
    {
        await _context.Equipment.AddAsync(equipment);
    }

    public async Task UpdateAsync(Equipment equipment)
    {
        _context.Equipment.Update(equipment);
    }

    public async Task DeleteAsync(Guid id)
    {
        var equipment = await _context.Equipment.FindAsync(id);
        if (equipment != null)
        {
            _context.Equipment.Remove(equipment);
        }
    }
}

using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Interfaces;
using FarmSync.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace FarmSync.Infrastructure.Repositories;

public class SupplierRepository : Repository<Supplier>, ISupplierRepository
{
    public SupplierRepository(FarmSyncDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Supplier>> GetActiveAsync()
    {
        return await _context.Suppliers
            .Where(s => s.IsActive)
            .OrderBy(s => s.Name)
            .ToListAsync();
    }

    public async Task<Supplier?> GetByNameAsync(string name)
    {
        return await _context.Suppliers
            .FirstOrDefaultAsync(s => s.Name.ToLower() == name.ToLower());
    }
}

using FarmSync.Domain.Common;
using FarmSync.Domain.Entities.Procurement;

namespace FarmSync.Domain.Interfaces;

public interface ISupplierRepository : IRepository<Supplier>
{
    Task<IEnumerable<Supplier>> GetActiveAsync();
    Task<Supplier?> GetByNameAsync(string name);
}

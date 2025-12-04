using FarmSync.Domain.Entities.Procurement;

namespace FarmSync.Domain.Interfaces;

public interface IPurchaseOrderRepository : IRepository<PurchaseOrder>
{
    Task<IEnumerable<PurchaseOrder>> GetAllWithDetailsAsync();
    Task<PurchaseOrder?> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<PurchaseOrder>> GetByStatusAsync(POStatus status);
    Task<IEnumerable<PurchaseOrder>> GetPendingApprovalsAsync();
    Task<string> GeneratePONumberAsync();
}

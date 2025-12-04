using FarmSync.Domain.Entities.Procurement;

namespace FarmSync.Domain.Interfaces;

public interface IGoodsReceivedRepository : IRepository<GoodsReceived>
{
    Task<IEnumerable<GoodsReceived>> GetAllWithDetailsAsync();
    Task<GoodsReceived?> GetByIdWithDetailsAsync(Guid id);
    Task<IEnumerable<GoodsReceived>> GetByPurchaseOrderAsync(Guid purchaseOrderId);
    Task<IEnumerable<GoodsReceived>> GetPendingApprovalsAsync();
    Task<string> GenerateReceiptNumberAsync();
}

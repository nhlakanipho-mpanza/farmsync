using FarmSync.Application.DTOs.Procurement;

namespace FarmSync.Application.Interfaces;

public interface IGoodsReceivedService
{
    Task<IEnumerable<GoodsReceivedDto>> GetAllAsync();
    Task<GoodsReceivedDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<GoodsReceivedDto>> GetByPurchaseOrderAsync(Guid purchaseOrderId);
    Task<IEnumerable<GoodsReceivedDto>> GetPendingApprovalsAsync();
    Task<GoodsReceivedDto> CreateAsync(CreateGoodsReceivedDto dto, Guid receivedBy);
    Task<GoodsReceivedDto> ApproveAsync(Guid id, Guid approvedBy);
    Task<GoodsReceivedDto> RejectAsync(Guid id, string reason);
}

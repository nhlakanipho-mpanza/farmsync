using FarmSync.Application.DTOs.Procurement;

namespace FarmSync.Application.Interfaces;

public interface IPurchaseOrderService
{
    Task<IEnumerable<PurchaseOrderDto>> GetAllAsync();
    Task<PurchaseOrderDto?> GetByIdAsync(Guid id);
    Task<IEnumerable<PurchaseOrderDto>> GetByStatusAsync(string status);
    Task<IEnumerable<PurchaseOrderDto>> GetPendingApprovalsAsync();
    Task<IEnumerable<PurchaseOrderDto>> GetAvailableForReceivingAsync();
    Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto dto, Guid createdBy);
    Task<PurchaseOrderDto> UpdateAsync(Guid id, UpdatePurchaseOrderDto dto);
    Task<PurchaseOrderDto> ApproveAsync(Guid id, Guid approvedBy);
    Task DeleteAsync(Guid id);
}

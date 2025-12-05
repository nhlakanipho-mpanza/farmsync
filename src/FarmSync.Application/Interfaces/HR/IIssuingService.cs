using FarmSync.Application.DTOs.HR;

namespace FarmSync.Application.Interfaces.HR;

public interface IIssuingService
{
    // Inventory Issuing
    Task<InventoryIssueDTO> RequestInventoryAsync(CreateInventoryIssueDTO dto, string requestedBy);
    Task<InventoryIssueDTO> ApproveInventoryIssueAsync(Guid id, ApproveInventoryIssueDTO dto, string approvedBy);
    Task<InventoryIssueDTO> ReturnInventoryAsync(Guid id, ReturnInventoryIssueDTO dto);
    Task<IEnumerable<InventoryIssueDTO>> GetPendingInventoryApprovalsAsync();
    Task<IEnumerable<InventoryIssueDTO>> GetInventoryIssuesByTeamAsync(Guid teamId);
    
    // Equipment Issuing
    Task<EquipmentIssueDTO> RequestEquipmentAsync(CreateEquipmentIssueDTO dto, string requestedBy);
    Task<EquipmentIssueDTO> ApproveEquipmentIssueAsync(Guid id, ApproveEquipmentIssueDTO dto, string approvedBy);
    Task<EquipmentIssueDTO> ReturnEquipmentAsync(Guid id, ReturnEquipmentIssueDTO dto);
    Task<IEnumerable<EquipmentIssueDTO>> GetPendingEquipmentApprovalsAsync();
    Task<IEnumerable<EquipmentIssueDTO>> GetEquipmentIssuesByTeamAsync(Guid teamId);
    Task<IEnumerable<EquipmentIssueDTO>> GetOverdueEquipmentReturnsAsync();
}

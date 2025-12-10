using FarmSync.Application.DTOs.Procurement;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Entities.Notifications;
using FarmSync.Domain.Entities.Auth;
using Microsoft.EntityFrameworkCore;

namespace FarmSync.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _poRepository;
    private readonly IRepository<Supplier> _supplierRepository;
    private readonly IRepository<PurchaseOrderItem> _poItemRepository;
    private readonly IRepository<Equipment> _equipmentRepository;
    private readonly INotificationService _notificationService;
    private readonly IRepository<UserRole> _userRoleRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderService(
        IPurchaseOrderRepository poRepository,
        IRepository<Supplier> supplierRepository,
        IRepository<PurchaseOrderItem> poItemRepository,
        IRepository<Equipment> equipmentRepository,
        INotificationService notificationService,
        IRepository<UserRole> userRoleRepository,
        IUnitOfWork unitOfWork)
    {
        _poRepository = poRepository;
        _supplierRepository = supplierRepository;
        _poItemRepository = poItemRepository;
        _equipmentRepository = equipmentRepository;
        _notificationService = notificationService;
        _userRoleRepository = userRoleRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetAllAsync()
    {
        var purchaseOrders = await _poRepository.GetAllWithDetailsAsync();
        return purchaseOrders.Select(MapToDto);
    }

    public async Task<PurchaseOrderDto?> GetByIdAsync(Guid id)
    {
        var purchaseOrder = await _poRepository.GetByIdWithDetailsAsync(id);
        return purchaseOrder == null ? null : MapToDto(purchaseOrder);
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetByStatusAsync(string status)
    {
        if (!Enum.TryParse<POStatus>(status, out var poStatus))
        {
            throw new ArgumentException($"Invalid status: {status}");
        }

        var purchaseOrders = await _poRepository.GetByStatusAsync(poStatus);
        return purchaseOrders.Select(MapToDto);
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetPendingApprovalsAsync()
    {
        var purchaseOrders = await _poRepository.GetPendingApprovalsAsync();
        return purchaseOrders.Select(MapToDto);
    }

    public async Task<IEnumerable<PurchaseOrderDto>> GetAvailableForReceivingAsync()
    {
        var approvedPOs = await _poRepository.GetByStatusAsync(POStatus.Approved);
        var partiallyReceivedPOs = await _poRepository.GetByStatusAsync(POStatus.PartiallyReceived);
        var allPOs = approvedPOs.Concat(partiallyReceivedPOs);
        return allPOs.Select(MapToDto);
    }

    public async Task<PurchaseOrderDto> CreateAsync(CreatePurchaseOrderDto dto, Guid createdBy)
    {
        // Validate supplier exists
        var supplier = await _supplierRepository.GetByIdAsync(dto.SupplierId);
        if (supplier == null)
        {
            throw new ArgumentException($"Supplier with ID {dto.SupplierId} not found");
        }

        // Generate PO number
        var poNumber = await _poRepository.GeneratePONumberAsync();

        // Calculate total amount
        var totalAmount = dto.Items.Sum(item => item.OrderedQuantity * item.UnitPrice);

        var purchaseOrder = new PurchaseOrder
        {
            PONumber = poNumber,
            SupplierId = dto.SupplierId,
            OrderDate = dto.OrderDate,
            ExpectedDeliveryDate = dto.ExpectedDeliveryDate,
            Status = POStatus.Created,
            TotalAmount = totalAmount,
            Notes = dto.Notes,
            CreatedBy = createdBy.ToString()
        };

        // Add items
        foreach (var itemDto in dto.Items)
        {
            // Validate that either InventoryItemId or EquipmentId is provided, but not both
            if (itemDto.InventoryItemId == null && itemDto.EquipmentId == null)
            {
                throw new ArgumentException("Each item must specify either InventoryItemId or EquipmentId");
            }
            if (itemDto.InventoryItemId != null && itemDto.EquipmentId != null)
            {
                throw new ArgumentException("Each item cannot have both InventoryItemId and EquipmentId");
            }

            purchaseOrder.Items.Add(new PurchaseOrderItem
            {
                InventoryItemId = itemDto.InventoryItemId,
                EquipmentId = itemDto.EquipmentId,
                OrderedQuantity = itemDto.OrderedQuantity,
                UnitPrice = itemDto.UnitPrice,
                Description = itemDto.Description,
                ReceivedQuantity = 0
            });
        }

        await _poRepository.AddAsync(purchaseOrder);
        await _unitOfWork.SaveChangesAsync();

        // Notify Accounting Managers about new PO pending approval
        var accountingManagerRoleId = await _userRoleRepository
            .GetAllAsync()
            .ContinueWith(t => t.Result.FirstOrDefault(ur => ur.Role.Name == "Accounting Manager")?.RoleId);

        if (accountingManagerRoleId.HasValue)
        {
            var accountingManagers = await _userRoleRepository
                .GetAllAsync()
                .ContinueWith(t => t.Result.Where(ur => ur.RoleId == accountingManagerRoleId.Value).ToList());

            foreach (var manager in accountingManagers)
            {
                await _notificationService.SendNotificationAsync(
                    manager.UserId,
                    NotificationType.PurchaseOrderStatusChanged,
                    "New Purchase Order Pending Approval",
                    $"Purchase Order {poNumber} has been created and requires your approval.",
                    $"/procurement/purchase-orders/{purchaseOrder.Id}",
                    null
                );
            }
        }

        return MapToDto(await _poRepository.GetByIdWithDetailsAsync(purchaseOrder.Id) 
            ?? throw new InvalidOperationException("Failed to retrieve created purchase order"));
    }

    public async Task<PurchaseOrderDto> UpdateAsync(Guid id, UpdatePurchaseOrderDto dto)
    {
        if (id != dto.Id)
        {
            throw new ArgumentException("ID mismatch");
        }

        var purchaseOrder = await _poRepository.GetByIdWithDetailsAsync(id);
        if (purchaseOrder == null)
        {
            throw new KeyNotFoundException($"Purchase order with ID {id} not found");
        }

        if (purchaseOrder.Status != POStatus.Created)
        {
            throw new InvalidOperationException("Only purchase orders in Created status can be updated");
        }

        // Update basic properties
        purchaseOrder.SupplierId = dto.SupplierId;
        purchaseOrder.ExpectedDeliveryDate = dto.ExpectedDeliveryDate;
        purchaseOrder.Notes = dto.Notes;

        // Remove existing items
        foreach (var item in purchaseOrder.Items.ToList())
        {
            await _poItemRepository.DeleteAsync(item.Id);
        }

        // Add new items
        var totalAmount = 0m;
        foreach (var itemDto in dto.Items)
        {
            // Validate that either InventoryItemId or EquipmentId is provided, but not both
            if (itemDto.InventoryItemId == null && itemDto.EquipmentId == null)
            {
                throw new ArgumentException("Each item must specify either InventoryItemId or EquipmentId");
            }
            if (itemDto.InventoryItemId != null && itemDto.EquipmentId != null)
            {
                throw new ArgumentException("Each item cannot have both InventoryItemId and EquipmentId");
            }

            var newItem = new PurchaseOrderItem
            {
                PurchaseOrderId = id,
                InventoryItemId = itemDto.InventoryItemId,
                EquipmentId = itemDto.EquipmentId,
                OrderedQuantity = itemDto.OrderedQuantity,
                UnitPrice = itemDto.UnitPrice,
                Description = itemDto.Description,
                ReceivedQuantity = 0
            };
            await _poItemRepository.AddAsync(newItem);
            totalAmount += itemDto.OrderedQuantity * itemDto.UnitPrice;
        }

        purchaseOrder.TotalAmount = totalAmount;
        purchaseOrder.UpdatedAt = DateTime.UtcNow;

        await _poRepository.UpdateAsync(purchaseOrder);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(await _poRepository.GetByIdWithDetailsAsync(id) 
            ?? throw new InvalidOperationException("Failed to retrieve updated purchase order"));
    }

    public async Task<PurchaseOrderDto> ApproveAsync(Guid id, Guid approvedBy)
    {
        var purchaseOrder = await _poRepository.GetByIdWithDetailsAsync(id);
        if (purchaseOrder == null)
        {
            throw new KeyNotFoundException($"Purchase order with ID {id} not found");
        }

        if (purchaseOrder.Status != POStatus.Created)
        {
            throw new InvalidOperationException("Only purchase orders in Created status can be approved");
        }

        purchaseOrder.Status = POStatus.Approved;
        purchaseOrder.ApprovedBy = approvedBy;
        purchaseOrder.ApprovedAt = DateTime.UtcNow;
        purchaseOrder.UpdatedAt = DateTime.UtcNow;

        await _poRepository.UpdateAsync(purchaseOrder);
        await _unitOfWork.SaveChangesAsync();

        // Notify the PO creator
        if (!string.IsNullOrEmpty(purchaseOrder.CreatedBy) && Guid.TryParse(purchaseOrder.CreatedBy, out var creatorId))
        {
            await _notificationService.SendNotificationAsync(
                creatorId,
                NotificationType.PurchaseOrderStatusChanged,
                "Purchase Order Approved",
                $"Your Purchase Order {purchaseOrder.PONumber} has been approved.",
                $"/procurement/purchase-orders/{purchaseOrder.Id}",
                null
            );
        }

        // Notify Operations Clerk to update expected delivery
        var operationsClerkRoleId = await _userRoleRepository
            .GetAllAsync()
            .ContinueWith(t => t.Result.FirstOrDefault(ur => ur.Role.Name == "Operations Clerk")?.RoleId);

        if (operationsClerkRoleId.HasValue)
        {
            var operationsClerks = await _userRoleRepository
                .GetAllAsync()
                .ContinueWith(t => t.Result.Where(ur => ur.RoleId == operationsClerkRoleId.Value).ToList());

            foreach (var clerk in operationsClerks)
            {
                await _notificationService.SendNotificationAsync(
                    clerk.UserId,
                    NotificationType.PurchaseOrderStatusChanged,
                    "Update Expected Delivery Date",
                    $"Purchase Order {purchaseOrder.PONumber} has been approved. Please contact the supplier and update the expected delivery date.",
                    $"/procurement/purchase-orders/{purchaseOrder.Id}/edit",
                    null
                );
            }
        }

        return MapToDto(purchaseOrder);
    }

    public async Task DeleteAsync(Guid id)
    {
        var purchaseOrder = await _poRepository.GetByIdAsync(id);
        if (purchaseOrder == null)
        {
            throw new KeyNotFoundException($"Purchase order with ID {id} not found");
        }

        if (purchaseOrder.Status != POStatus.Created)
        {
            throw new InvalidOperationException("Only purchase orders in Created status can be deleted");
        }

        await _poRepository.DeleteAsync(purchaseOrder.Id);
        await _unitOfWork.SaveChangesAsync();
    }

    private PurchaseOrderDto MapToDto(PurchaseOrder po)
    {
        return new PurchaseOrderDto
        {
            Id = po.Id,
            PONumber = po.PONumber,
            SupplierId = po.SupplierId,
            SupplierName = po.Supplier?.Name ?? string.Empty,
            OrderDate = po.OrderDate,
            ExpectedDeliveryDate = po.ExpectedDeliveryDate,
            Status = po.Status.ToString(),
            TotalAmount = po.TotalAmount,
            Notes = po.Notes,
            ApprovedBy = po.ApprovedBy,
            ApprovedAt = po.ApprovedAt,
            Items = po.Items.Select(item => new PurchaseOrderItemDto
            {
                Id = item.Id,
                PurchaseOrderId = item.PurchaseOrderId,
                InventoryItemId = item.InventoryItemId,
                EquipmentId = item.EquipmentId,
                ItemName = item.InventoryItem?.Name ?? item.Equipment?.Name ?? string.Empty,
                ItemSKU = item.InventoryItem?.SKU ?? item.Equipment?.SerialNumber ?? string.Empty,
                ItemType = item.InventoryItemId.HasValue ? \"Inventory\" : \"Equipment\",
                OrderedQuantity = item.OrderedQuantity,
                ReceivedQuantity = item.ReceivedQuantity,
                UnitPrice = item.UnitPrice,
                Description = item.Description,
                Notes = item.Notes
            }).ToList()
        };
    }
}

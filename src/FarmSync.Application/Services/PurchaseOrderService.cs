using FarmSync.Application.DTOs.Procurement;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Application.Services;

public class PurchaseOrderService : IPurchaseOrderService
{
    private readonly IPurchaseOrderRepository _poRepository;
    private readonly IRepository<Supplier> _supplierRepository;
    private readonly IRepository<PurchaseOrderItem> _poItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public PurchaseOrderService(
        IPurchaseOrderRepository poRepository,
        IRepository<Supplier> supplierRepository,
        IRepository<PurchaseOrderItem> poItemRepository,
        IUnitOfWork unitOfWork)
    {
        _poRepository = poRepository;
        _supplierRepository = supplierRepository;
        _poItemRepository = poItemRepository;
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
            purchaseOrder.Items.Add(new PurchaseOrderItem
            {
                InventoryItemId = itemDto.InventoryItemId,
                OrderedQuantity = itemDto.OrderedQuantity,
                UnitPrice = itemDto.UnitPrice,
                Description = itemDto.Description,
                ReceivedQuantity = 0
            });
        }

        await _poRepository.AddAsync(purchaseOrder);
        await _unitOfWork.SaveChangesAsync();

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
            var newItem = new PurchaseOrderItem
            {
                PurchaseOrderId = id,
                InventoryItemId = itemDto.InventoryItemId,
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
                ItemName = item.InventoryItem?.Name ?? string.Empty,
                ItemSKU = item.InventoryItem?.SKU ?? string.Empty,
                OrderedQuantity = item.OrderedQuantity,
                ReceivedQuantity = item.ReceivedQuantity,
                UnitPrice = item.UnitPrice,
                Description = item.Description,
                Notes = item.Notes
            }).ToList()
        };
    }
}

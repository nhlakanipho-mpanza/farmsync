using FarmSync.Application.DTOs.Procurement;
using FarmSync.Application.Interfaces;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Entities.Procurement;
using FarmSync.Domain.Interfaces;

namespace FarmSync.Application.Services;

public class GoodsReceivedService : IGoodsReceivedService
{
    private readonly IGoodsReceivedRepository _grRepository;
    private readonly IPurchaseOrderRepository _poRepository;
    private readonly IRepository<GoodsReceivedItem> _grItemRepository;
    private readonly IRepository<StockLevel> _stockLevelRepository;
    private readonly IRepository<InventoryLocation> _locationRepository;
    private readonly IRepository<InventoryItem> _inventoryItemRepository;
    private readonly IUnitOfWork _unitOfWork;

    public GoodsReceivedService(
        IGoodsReceivedRepository grRepository,
        IPurchaseOrderRepository poRepository,
        IRepository<GoodsReceivedItem> grItemRepository,
        IRepository<StockLevel> stockLevelRepository,
        IRepository<InventoryLocation> locationRepository,
        IRepository<InventoryItem> inventoryItemRepository,
        IUnitOfWork unitOfWork)
    {
        _grRepository = grRepository;
        _poRepository = poRepository;
        _grItemRepository = grItemRepository;
        _stockLevelRepository = stockLevelRepository;
        _locationRepository = locationRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<IEnumerable<GoodsReceivedDto>> GetAllAsync()
    {
        var goodsReceivedRecords = await _grRepository.GetAllWithDetailsAsync();
        return goodsReceivedRecords.Select(MapToDto);
    }

    public async Task<GoodsReceivedDto?> GetByIdAsync(Guid id)
    {
        var goodsReceived = await _grRepository.GetByIdWithDetailsAsync(id);
        return goodsReceived == null ? null : MapToDto(goodsReceived);
    }

    public async Task<IEnumerable<GoodsReceivedDto>> GetByPurchaseOrderAsync(Guid purchaseOrderId)
    {
        var goodsReceivedRecords = await _grRepository.GetByPurchaseOrderAsync(purchaseOrderId);
        return goodsReceivedRecords.Select(MapToDto);
    }

    public async Task<IEnumerable<GoodsReceivedDto>> GetPendingApprovalsAsync()
    {
        var goodsReceivedRecords = await _grRepository.GetPendingApprovalsAsync();
        return goodsReceivedRecords.Select(MapToDto);
    }

    public async Task<GoodsReceivedDto> CreateAsync(CreateGoodsReceivedDto dto, Guid receivedBy)
    {
        // Validate purchase order exists
        var purchaseOrder = await _poRepository.GetByIdWithDetailsAsync(dto.PurchaseOrderId);
        if (purchaseOrder == null)
        {
            throw new KeyNotFoundException($"Purchase order with ID {dto.PurchaseOrderId} not found");
        }

        if (purchaseOrder.Status != POStatus.Created && purchaseOrder.Status != POStatus.Approved && purchaseOrder.Status != POStatus.PartiallyReceived)
        {
            throw new InvalidOperationException("Only created or approved purchase orders can receive goods");
        }

        // Generate receipt number
        var receiptNumber = await _grRepository.GenerateReceiptNumberAsync();

        // Check for discrepancies
        var hasDiscrepancies = dto.Items.Any(item => 
            item.QuantityDamaged > 0 || item.QuantityShortfall > 0);

        var goodsReceived = new GoodsReceived
        {
            ReceiptNumber = receiptNumber,
            PurchaseOrderId = dto.PurchaseOrderId,
            ReceivedDate = dto.ReceivedDate,
            ReceivedBy = receivedBy,
            Status = hasDiscrepancies ? GRStatus.Pending : GRStatus.Approved,
            HasDiscrepancies = hasDiscrepancies,
            DiscrepancyNotes = dto.DiscrepancyNotes,
            IsFinalReceipt = dto.IsFinalReceipt ?? false
        };

        // Add items
        foreach (var itemDto in dto.Items)
        {
            goodsReceived.Items.Add(new GoodsReceivedItem
            {
                PurchaseOrderItemId = itemDto.PurchaseOrderItemId,
                QuantityReceived = itemDto.QuantityReceived,
                QuantityDamaged = itemDto.QuantityDamaged,
                QuantityShortfall = itemDto.QuantityShortfall,
                Condition = itemDto.Condition,
                Notes = itemDto.Notes
            });
        }

        await _grRepository.AddAsync(goodsReceived);
        await _unitOfWork.SaveChangesAsync();

        // If no discrepancies, auto-approve and update stock
        if (!hasDiscrepancies)
        {
            await UpdateStockLevelsAsync(goodsReceived);
            await UpdatePurchaseOrderStatus(purchaseOrder, dto.Items, dto.IsFinalReceipt ?? false);
        }
        else
        {
            // For receipts with discrepancies, just update PO received quantities (not status)
            await UpdatePurchaseOrderReceivedQuantities(purchaseOrder, dto.Items);
        }

        return MapToDto(await _grRepository.GetByIdWithDetailsAsync(goodsReceived.Id) 
            ?? throw new InvalidOperationException("Failed to retrieve created goods received record"));
    }

    public async Task<GoodsReceivedDto> ApproveAsync(Guid id, Guid approvedBy)
    {
        var goodsReceived = await _grRepository.GetByIdWithDetailsAsync(id);
        if (goodsReceived == null)
        {
            throw new KeyNotFoundException($"Goods received record with ID {id} not found");
        }

        if (goodsReceived.Status != GRStatus.Pending)
        {
            throw new InvalidOperationException("Only pending goods received records can be approved");
        }

        goodsReceived.Status = GRStatus.Approved;
        goodsReceived.ApprovedBy = approvedBy;
        goodsReceived.ApprovedAt = DateTime.UtcNow;
        goodsReceived.UpdatedAt = DateTime.UtcNow;

        await _grRepository.UpdateAsync(goodsReceived);
        await _unitOfWork.SaveChangesAsync();

        // Update stock levels after approval
        await UpdateStockLevelsAsync(goodsReceived);

        // Update PO status
        var purchaseOrder = await _poRepository.GetByIdWithDetailsAsync(goodsReceived.PurchaseOrderId);
        if (purchaseOrder != null)
        {
            // Use the IsFinalReceipt flag from the entity
            bool isFinalReceipt = goodsReceived.IsFinalReceipt;
            
            var grItems = goodsReceived.Items.Select(gri => new CreateGoodsReceivedItemDto
            {
                PurchaseOrderItemId = gri.PurchaseOrderItemId,
                QuantityReceived = gri.QuantityReceived,
                QuantityDamaged = gri.QuantityDamaged,
                QuantityShortfall = gri.QuantityShortfall
            }).ToList();

            await UpdatePurchaseOrderStatus(purchaseOrder, grItems, isFinalReceipt);
        }

        return MapToDto(goodsReceived);
    }

    public async Task<GoodsReceivedDto> RejectAsync(Guid id, string reason)
    {
        var goodsReceived = await _grRepository.GetByIdWithDetailsAsync(id);
        if (goodsReceived == null)
        {
            throw new KeyNotFoundException($"Goods received record with ID {id} not found");
        }

        if (goodsReceived.Status != GRStatus.Pending)
        {
            throw new InvalidOperationException("Only pending goods received records can be rejected");
        }

        goodsReceived.Status = GRStatus.Rejected;
        goodsReceived.DiscrepancyNotes = $"{goodsReceived.DiscrepancyNotes}\\nRejection Reason: {reason}";
        goodsReceived.UpdatedAt = DateTime.UtcNow;

        await _grRepository.UpdateAsync(goodsReceived);
        await _unitOfWork.SaveChangesAsync();

        return MapToDto(goodsReceived);
    }

    private async Task UpdateStockLevelsAsync(GoodsReceived goodsReceived)
    {
        // Load the purchase order once with all items
        var purchaseOrder = await _poRepository.GetByIdWithDetailsAsync(goodsReceived.PurchaseOrderId);
        if (purchaseOrder == null) return;

        // Get the default inventory location (or first available)
        var locations = await _locationRepository.GetAllAsync();
        var defaultLocation = locations.FirstOrDefault();
        if (defaultLocation == null)
        {
            throw new InvalidOperationException("No inventory location found. Please create at least one location before receiving goods.");
        }

        foreach (var item in goodsReceived.Items)
        {
            var poItem = purchaseOrder.Items.FirstOrDefault(i => i.Id == item.PurchaseOrderItemId);
            if (poItem?.InventoryItemId == null) continue;

            // Calculate actual received quantity (excluding damaged/shortfall)
            var actualReceived = item.QuantityReceived - item.QuantityDamaged;
            if (actualReceived <= 0) continue;

            // Get the inventory item to update weighted average cost
            var inventoryItem = await _inventoryItemRepository.GetByIdAsync(poItem.InventoryItemId);
            if (inventoryItem == null) continue;

            // Find existing stock level for this inventory item
            var stockLevel = (await _stockLevelRepository.FindAsync(sl => sl.InventoryItemId == poItem.InventoryItemId))
                .FirstOrDefault();

            decimal oldQuantity = stockLevel?.Quantity ?? 0;
            decimal oldValue = oldQuantity * inventoryItem.AverageUnitCost;
            decimal newPurchaseValue = actualReceived * poItem.UnitPrice;
            decimal newTotalQuantity = oldQuantity + actualReceived;

            // Calculate weighted average cost
            // Formula: New Avg Cost = (Old Value + New Purchase Value) / New Total Quantity
            if (newTotalQuantity > 0)
            {
                inventoryItem.AverageUnitCost = (oldValue + newPurchaseValue) / newTotalQuantity;
                inventoryItem.UpdatedAt = DateTime.UtcNow;
                await _inventoryItemRepository.UpdateAsync(inventoryItem);
            }

            if (stockLevel == null)
            {
                // Create new stock level with the received quantity
                stockLevel = new StockLevel
                {
                    InventoryItemId = poItem.InventoryItemId,
                    LocationId = defaultLocation.Id,
                    Quantity = actualReceived,
                    UpdatedAt = DateTime.UtcNow
                };
                await _stockLevelRepository.AddAsync(stockLevel);
            }
            else
            {
                // Update existing stock level
                stockLevel.Quantity += actualReceived;
                stockLevel.UpdatedAt = DateTime.UtcNow;
                await _stockLevelRepository.UpdateAsync(stockLevel);
            }
        }

        await _unitOfWork.SaveChangesAsync();
    }

    private async Task UpdatePurchaseOrderStatus(PurchaseOrder purchaseOrder, List<CreateGoodsReceivedItemDto> receivedItems, bool isFinalReceipt)
    {
        // Update received quantities on PO items
        foreach (var receivedItem in receivedItems)
        {
            var poItem = purchaseOrder.Items.FirstOrDefault(i => i.Id == receivedItem.PurchaseOrderItemId);
            if (poItem != null)
            {
                var actualReceived = receivedItem.QuantityReceived - receivedItem.QuantityDamaged;
                poItem.ReceivedQuantity += actualReceived;
            }
        }

        // Determine PO status based on received quantities and final receipt flag
        var allItemsFullyReceived = purchaseOrder.Items.All(i => i.ReceivedQuantity >= i.OrderedQuantity);
        var anyItemPartiallyReceived = purchaseOrder.Items.Any(i => i.ReceivedQuantity > 0);

        if (isFinalReceipt)
        {
            // If marked as final receipt, close the PO (with or without issues)
            var hasShortfall = purchaseOrder.Items.Any(i => i.ReceivedQuantity < i.OrderedQuantity);
            purchaseOrder.Status = hasShortfall ? POStatus.ClosedWithIssues : POStatus.FullyReceived;
        }
        else if (allItemsFullyReceived)
        {
            purchaseOrder.Status = POStatus.FullyReceived;
        }
        else if (anyItemPartiallyReceived)
        {
            purchaseOrder.Status = POStatus.PartiallyReceived;
        }

        purchaseOrder.UpdatedAt = DateTime.UtcNow;
        await _poRepository.UpdateAsync(purchaseOrder);
        await _unitOfWork.SaveChangesAsync();
    }

    private async Task UpdatePurchaseOrderReceivedQuantities(PurchaseOrder purchaseOrder, List<CreateGoodsReceivedItemDto> receivedItems)
    {
        // Only update received quantities, don't change status (used when GR is pending approval)
        foreach (var receivedItem in receivedItems)
        {
            var poItem = purchaseOrder.Items.FirstOrDefault(i => i.Id == receivedItem.PurchaseOrderItemId);
            if (poItem != null)
            {
                var actualReceived = receivedItem.QuantityReceived - receivedItem.QuantityDamaged;
                poItem.ReceivedQuantity += actualReceived;
            }
        }

        purchaseOrder.UpdatedAt = DateTime.UtcNow;
        await _poRepository.UpdateAsync(purchaseOrder);
        await _unitOfWork.SaveChangesAsync();
    }

    private GoodsReceivedDto MapToDto(GoodsReceived gr)
    {
        return new GoodsReceivedDto
        {
            Id = gr.Id,
            ReceiptNumber = gr.ReceiptNumber,
            PurchaseOrderId = gr.PurchaseOrderId,
            PONumber = gr.PurchaseOrder?.PONumber ?? string.Empty,
            ReceivedDate = gr.ReceivedDate,
            ReceivedBy = gr.ReceivedBy,
            Status = gr.Status.ToString(),
            HasDiscrepancies = gr.HasDiscrepancies,
            DiscrepancyNotes = gr.DiscrepancyNotes,
            IsFinalReceipt = gr.IsFinalReceipt,
            ApprovedBy = gr.ApprovedBy,
            ApprovedAt = gr.ApprovedAt,
            Items = gr.Items.Select(item => new GoodsReceivedItemDto
            {
                Id = item.Id,
                GoodsReceivedId = item.GoodsReceivedId,
                PurchaseOrderItemId = item.PurchaseOrderItemId,
                ItemName = item.PurchaseOrderItem?.InventoryItem?.Name ?? string.Empty,
                OrderedQuantity = item.PurchaseOrderItem?.OrderedQuantity ?? 0,
                QuantityReceived = item.QuantityReceived,
                QuantityDamaged = item.QuantityDamaged,
                QuantityShortfall = item.QuantityShortfall,
                Condition = item.Condition,
                Notes = item.Notes
            }).ToList()
        };
    }
}

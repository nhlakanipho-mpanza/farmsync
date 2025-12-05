using FarmSync.Application.DTOs.HR;
using FarmSync.Application.Interfaces.HR;
using FarmSync.Domain.Entities.HR;
using FarmSync.Domain.Entities.Inventory;
using FarmSync.Domain.Interfaces;
using FarmSync.Domain.Interfaces.HR;

namespace FarmSync.Application.Services.HR;

public class IssuingService : IIssuingService
{
    private readonly IInventoryIssueRepository _inventoryIssueRepository;
    private readonly IEquipmentIssueRepository _equipmentIssueRepository;
    private readonly IRepository<InventoryItem> _inventoryItemRepository;
    private readonly IRepository<Equipment> _equipmentRepository;
    private readonly IRepository<IssueStatus> _issueStatusRepository;
    private readonly IUnitOfWork _unitOfWork;

    public IssuingService(
        IInventoryIssueRepository inventoryIssueRepository,
        IEquipmentIssueRepository equipmentIssueRepository,
        IRepository<InventoryItem> inventoryItemRepository,
        IRepository<Equipment> equipmentRepository,
        IRepository<IssueStatus> issueStatusRepository,
        IUnitOfWork unitOfWork)
    {
        _inventoryIssueRepository = inventoryIssueRepository;
        _equipmentIssueRepository = equipmentIssueRepository;
        _inventoryItemRepository = inventoryItemRepository;
        _equipmentRepository = equipmentRepository;
        _issueStatusRepository = issueStatusRepository;
        _unitOfWork = unitOfWork;
    }

    // Inventory Issuing
    public async Task<InventoryIssueDTO> RequestInventoryAsync(CreateInventoryIssueDTO dto, string requestedBy)
    {
        var pendingStatus = (await _issueStatusRepository.GetAllAsync())
            .FirstOrDefault(s => s.Name == "Pending");

        if (pendingStatus == null)
        {
            throw new InvalidOperationException("Pending status not found in system");
        }

        var issueNumber = await _inventoryIssueRepository.GenerateIssueNumberAsync();

        var issue = new InventoryIssue
        {
            Id = Guid.NewGuid(),
            IssueNumber = issueNumber,
            InventoryItemId = dto.InventoryItemId,
            Quantity = dto.Quantity,
            WorkTaskId = dto.WorkTaskId,
            TeamId = dto.TeamId,
            EmployeeId = dto.EmployeeId,
            IssueStatusId = pendingStatus.Id,
            Purpose = dto.Purpose,
            IssuedDate = DateTime.UtcNow.AddHours(2),
            IssuedBy = requestedBy,
            Notes = dto.Notes
        };

        await _inventoryIssueRepository.AddAsync(issue);
        await _unitOfWork.SaveChangesAsync();

        return await MapInventoryIssueToDtoAsync(issue);
    }

    public async Task<InventoryIssueDTO> ApproveInventoryIssueAsync(Guid id, ApproveInventoryIssueDTO dto, string approvedBy)
    {
        var issue = await _inventoryIssueRepository.GetByIdAsync(id);
        if (issue == null)
        {
            throw new KeyNotFoundException($"Inventory issue with ID {id} not found");
        }

        var statusName = dto.Approved ? "Approved" : "Cancelled";
        var status = (await _issueStatusRepository.GetAllAsync())
            .FirstOrDefault(s => s.Name == statusName);

        if (status == null)
        {
            throw new InvalidOperationException($"{statusName} status not found in system");
        }

        issue.IssueStatusId = status.Id;
        issue.ApprovedDate = DateTime.UtcNow.AddHours(2);
        issue.ApprovedBy = approvedBy;
        if (!string.IsNullOrEmpty(dto.Notes))
        {
            issue.Notes = dto.Notes;
        }

        // If approved, reduce inventory stock
        if (dto.Approved)
        {
            var item = await _inventoryItemRepository.GetByIdAsync(issue.InventoryItemId);
            if (item == null)
            {
                throw new InvalidOperationException("Inventory item not found");
            }

            if (item.CurrentStockLevel < issue.Quantity)
            {
                throw new InvalidOperationException($"Insufficient stock. Available: {item.CurrentStockLevel}, Requested: {issue.Quantity}");
            }

            item.CurrentStockLevel -= issue.Quantity;
            await _inventoryItemRepository.UpdateAsync(item);

            // Update status to Issued
            var issuedStatus = (await _issueStatusRepository.GetAllAsync())
                .FirstOrDefault(s => s.Name == "Issued");
            if (issuedStatus != null)
            {
                issue.IssueStatusId = issuedStatus.Id;
            }
        }

        await _inventoryIssueRepository.UpdateAsync(issue);
        await _unitOfWork.SaveChangesAsync();

        return await MapInventoryIssueToDtoAsync(issue);
    }

    public async Task<InventoryIssueDTO> ReturnInventoryAsync(Guid id, ReturnInventoryIssueDTO dto)
    {
        var issue = await _inventoryIssueRepository.GetByIdAsync(id);
        if (issue == null)
        {
            throw new KeyNotFoundException($"Inventory issue with ID {id} not found");
        }

        issue.ReturnedQuantity = dto.ReturnedQuantity;
        issue.ReturnedDate = DateTime.UtcNow.AddHours(2);
        if (!string.IsNullOrEmpty(dto.Notes))
        {
            issue.Notes += $"\nReturn: {dto.Notes}";
        }

        // Return stock to inventory
        var item = await _inventoryItemRepository.GetByIdAsync(issue.InventoryItemId);
        if (item != null)
        {
            item.CurrentStockLevel += dto.ReturnedQuantity;
            await _inventoryItemRepository.UpdateAsync(item);
        }

        // Update status to Returned if fully returned
        if (dto.ReturnedQuantity >= issue.Quantity)
        {
            var returnedStatus = (await _issueStatusRepository.GetAllAsync())
                .FirstOrDefault(s => s.Name == "Returned");
            if (returnedStatus != null)
            {
                issue.IssueStatusId = returnedStatus.Id;
            }
        }

        await _inventoryIssueRepository.UpdateAsync(issue);
        await _unitOfWork.SaveChangesAsync();

        return await MapInventoryIssueToDtoAsync(issue);
    }

    public async Task<IEnumerable<InventoryIssueDTO>> GetPendingInventoryApprovalsAsync()
    {
        var issues = await _inventoryIssueRepository.GetPendingApprovalsAsync();
        var dtos = new List<InventoryIssueDTO>();
        foreach (var issue in issues)
        {
            dtos.Add(await MapInventoryIssueToDtoAsync(issue));
        }
        return dtos;
    }

    public async Task<IEnumerable<InventoryIssueDTO>> GetInventoryIssuesByTeamAsync(Guid teamId)
    {
        var issues = await _inventoryIssueRepository.GetByTeamAsync(teamId);
        var dtos = new List<InventoryIssueDTO>();
        foreach (var issue in issues)
        {
            dtos.Add(await MapInventoryIssueToDtoAsync(issue));
        }
        return dtos;
    }

    // Equipment Issuing
    public async Task<EquipmentIssueDTO> RequestEquipmentAsync(CreateEquipmentIssueDTO dto, string requestedBy)
    {
        var pendingStatus = (await _issueStatusRepository.GetAllAsync())
            .FirstOrDefault(s => s.Name == "Pending");

        if (pendingStatus == null)
        {
            throw new InvalidOperationException("Pending status not found in system");
        }

        var issueNumber = await _equipmentIssueRepository.GenerateIssueNumberAsync();

        var issue = new EquipmentIssue
        {
            Id = Guid.NewGuid(),
            IssueNumber = issueNumber,
            EquipmentId = dto.EquipmentId,
            WorkTaskId = dto.WorkTaskId,
            TeamId = dto.TeamId,
            EmployeeId = dto.EmployeeId,
            IssueStatusId = pendingStatus.Id,
            Purpose = dto.Purpose,
            IssuedDate = DateTime.UtcNow.AddHours(2),
            IssuedBy = requestedBy,
            ExpectedReturnDate = dto.ExpectedReturnDate,
            Notes = dto.Notes
        };

        await _equipmentIssueRepository.AddAsync(issue);
        await _unitOfWork.SaveChangesAsync();

        return await MapEquipmentIssueToDtoAsync(issue);
    }

    public async Task<EquipmentIssueDTO> ApproveEquipmentIssueAsync(Guid id, ApproveEquipmentIssueDTO dto, string approvedBy)
    {
        var issue = await _equipmentIssueRepository.GetByIdAsync(id);
        if (issue == null)
        {
            throw new KeyNotFoundException($"Equipment issue with ID {id} not found");
        }

        var statusName = dto.Approved ? "Issued" : "Cancelled";
        var status = (await _issueStatusRepository.GetAllAsync())
            .FirstOrDefault(s => s.Name == statusName);

        if (status == null)
        {
            throw new InvalidOperationException($"{statusName} status not found in system");
        }

        issue.IssueStatusId = status.Id;
        issue.ApprovedDate = DateTime.UtcNow.AddHours(2);
        issue.ApprovedBy = approvedBy;
        if (!string.IsNullOrEmpty(dto.Notes))
        {
            issue.Notes = dto.Notes;
        }

        await _equipmentIssueRepository.UpdateAsync(issue);
        await _unitOfWork.SaveChangesAsync();

        return await MapEquipmentIssueToDtoAsync(issue);
    }

    public async Task<EquipmentIssueDTO> ReturnEquipmentAsync(Guid id, ReturnEquipmentIssueDTO dto)
    {
        var issue = await _equipmentIssueRepository.GetByIdAsync(id);
        if (issue == null)
        {
            throw new KeyNotFoundException($"Equipment issue with ID {id} not found");
        }

        issue.ActualReturnDate = DateTime.UtcNow.AddHours(2);
        issue.ReturnCondition = dto.ReturnCondition;
        if (!string.IsNullOrEmpty(dto.Notes))
        {
            issue.Notes += $"\nReturn: {dto.Notes}";
        }

        var returnedStatus = (await _issueStatusRepository.GetAllAsync())
            .FirstOrDefault(s => s.Name == "Returned");
        if (returnedStatus != null)
        {
            issue.IssueStatusId = returnedStatus.Id;
        }

        await _equipmentIssueRepository.UpdateAsync(issue);
        await _unitOfWork.SaveChangesAsync();

        return await MapEquipmentIssueToDtoAsync(issue);
    }

    public async Task<IEnumerable<EquipmentIssueDTO>> GetPendingEquipmentApprovalsAsync()
    {
        var issues = await _equipmentIssueRepository.GetPendingApprovalsAsync();
        var dtos = new List<EquipmentIssueDTO>();
        foreach (var issue in issues)
        {
            dtos.Add(await MapEquipmentIssueToDtoAsync(issue));
        }
        return dtos;
    }

    public async Task<IEnumerable<EquipmentIssueDTO>> GetEquipmentIssuesByTeamAsync(Guid teamId)
    {
        var issues = await _equipmentIssueRepository.GetByTeamAsync(teamId);
        var dtos = new List<EquipmentIssueDTO>();
        foreach (var issue in issues)
        {
            dtos.Add(await MapEquipmentIssueToDtoAsync(issue));
        }
        return dtos;
    }

    public async Task<IEnumerable<EquipmentIssueDTO>> GetOverdueEquipmentReturnsAsync()
    {
        var issues = await _equipmentIssueRepository.GetOverdueReturnsAsync();
        var dtos = new List<EquipmentIssueDTO>();
        foreach (var issue in issues)
        {
            dtos.Add(await MapEquipmentIssueToDtoAsync(issue));
        }
        return dtos;
    }

    private async Task<InventoryIssueDTO> MapInventoryIssueToDtoAsync(InventoryIssue issue)
    {
        var item = await _inventoryItemRepository.GetByIdAsync(issue.InventoryItemId);
        var status = await _issueStatusRepository.GetByIdAsync(issue.IssueStatusId);

        return new InventoryIssueDTO
        {
            Id = issue.Id,
            IssueNumber = issue.IssueNumber,
            InventoryItemId = issue.InventoryItemId,
            InventoryItemName = item?.Name ?? "",
            ItemSKU = item?.SKU,
            Quantity = issue.Quantity,
            WorkTaskId = issue.WorkTaskId,
            TeamId = issue.TeamId,
            EmployeeId = issue.EmployeeId,
            IssueStatusId = issue.IssueStatusId,
            IssueStatusName = status?.Name ?? "",
            Purpose = issue.Purpose,
            IssuedDate = issue.IssuedDate,
            IssuedBy = issue.IssuedBy,
            ApprovedDate = issue.ApprovedDate,
            ApprovedBy = issue.ApprovedBy,
            ReturnedQuantity = issue.ReturnedQuantity,
            ReturnedDate = issue.ReturnedDate,
            Notes = issue.Notes,
            CreatedAt = issue.CreatedAt,
            UpdatedAt = issue.UpdatedAt
        };
    }

    private async Task<EquipmentIssueDTO> MapEquipmentIssueToDtoAsync(EquipmentIssue issue)
    {
        var equipment = await _equipmentRepository.GetByIdAsync(issue.EquipmentId);
        var status = await _issueStatusRepository.GetByIdAsync(issue.IssueStatusId);

        return new EquipmentIssueDTO
        {
            Id = issue.Id,
            IssueNumber = issue.IssueNumber,
            EquipmentId = issue.EquipmentId,
            EquipmentName = equipment?.Name ?? "",
            WorkTaskId = issue.WorkTaskId,
            TeamId = issue.TeamId,
            EmployeeId = issue.EmployeeId,
            IssueStatusId = issue.IssueStatusId,
            IssueStatusName = status?.Name ?? "",
            Purpose = issue.Purpose,
            IssuedDate = issue.IssuedDate,
            IssuedBy = issue.IssuedBy,
            ApprovedDate = issue.ApprovedDate,
            ApprovedBy = issue.ApprovedBy,
            ExpectedReturnDate = issue.ExpectedReturnDate,
            ActualReturnDate = issue.ActualReturnDate,
            ReturnCondition = issue.ReturnCondition,
            Notes = issue.Notes,
            CreatedAt = issue.CreatedAt,
            UpdatedAt = issue.UpdatedAt
        };
    }
}

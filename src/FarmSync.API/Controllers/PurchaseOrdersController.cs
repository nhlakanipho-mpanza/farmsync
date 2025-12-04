using FarmSync.Application.DTOs.Procurement;
using FarmSync.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace FarmSync.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PurchaseOrdersController : ControllerBase
{
    private readonly IPurchaseOrderService _service;
    private readonly ILogger<PurchaseOrdersController> _logger;

    public PurchaseOrdersController(IPurchaseOrderService service, ILogger<PurchaseOrdersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        try
        {
            var purchaseOrders = await _service.GetAllAsync();
            return Ok(purchaseOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting all purchase orders");
            return StatusCode(500, new { message = "An error occurred while fetching purchase orders" });
        }
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById(Guid id)
    {
        try
        {
            var purchaseOrder = await _service.GetByIdAsync(id);
            if (purchaseOrder == null)
                return NotFound(new { message = $"Purchase order with ID {id} not found" });

            return Ok(purchaseOrder);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting purchase order {Id}", id);
            return StatusCode(500, new { message = "An error occurred while fetching the purchase order" });
        }
    }

    [HttpGet("status/{status}")]
    public async Task<IActionResult> GetByStatus(string status)
    {
        try
        {
            var purchaseOrders = await _service.GetByStatusAsync(status);
            return Ok(purchaseOrders);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting purchase orders by status {Status}", status);
            return StatusCode(500, new { message = "An error occurred while fetching purchase orders" });
        }
    }

    [HttpGet("pending-approvals")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> GetPendingApprovals()
    {
        try
        {
            var purchaseOrders = await _service.GetPendingApprovalsAsync();
            return Ok(purchaseOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting pending approval purchase orders");
            return StatusCode(500, new { message = "An error occurred while fetching pending approvals" });
        }
    }

    [HttpGet("available-for-receiving")]
    public async Task<IActionResult> GetAvailableForReceiving()
    {
        try
        {
            var purchaseOrders = await _service.GetAvailableForReceivingAsync();
            return Ok(purchaseOrders);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error getting purchase orders available for receiving");
            return StatusCode(500, new { message = "An error occurred while fetching available purchase orders" });
        }
    }

    [HttpPost]
    [Authorize(Roles = "Accountant,Admin")]
    public async Task<IActionResult> Create([FromBody] CreatePurchaseOrderDto dto)
    {
        try
        {
            var userId = GetCurrentUserId();
            var purchaseOrder = await _service.CreateAsync(dto, userId);
            return CreatedAtAction(nameof(GetById), new { id = purchaseOrder.Id }, purchaseOrder);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error creating purchase order");
            return StatusCode(500, new { message = "An error occurred while creating the purchase order" });
        }
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Accountant,Admin")]
    public async Task<IActionResult> Update(Guid id, [FromBody] UpdatePurchaseOrderDto dto)
    {
        try
        {
            if (id != dto.Id)
                return BadRequest(new { message = "ID mismatch" });

            var purchaseOrder = await _service.UpdateAsync(id, dto);
            return Ok(purchaseOrder);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (ArgumentException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error updating purchase order {Id}", id);
            return StatusCode(500, new { message = "An error occurred while updating the purchase order" });
        }
    }

    [HttpPost("{id}/approve")]
    [Authorize(Roles = "Manager,Admin")]
    public async Task<IActionResult> Approve(Guid id)
    {
        try
        {
            var userId = GetCurrentUserId();
            var purchaseOrder = await _service.ApproveAsync(id, userId);
            return Ok(purchaseOrder);
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error approving purchase order {Id}", id);
            return StatusCode(500, new { message = "An error occurred while approving the purchase order" });
        }
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Accountant,Admin")]
    public async Task<IActionResult> Delete(Guid id)
    {
        try
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException ex)
        {
            return NotFound(new { message = ex.Message });
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error deleting purchase order {Id}", id);
            return StatusCode(500, new { message = "An error occurred while deleting the purchase order" });
        }
    }

    private Guid GetCurrentUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
        {
            throw new UnauthorizedAccessException("User ID not found in token");
        }
        return userId;
    }
}
